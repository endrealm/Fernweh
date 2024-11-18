using System.Collections.Generic;
using System.Linq;
using Core.Saving;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Modding;
using Core.Scripting.Saving;
using Core.States;
using Core.Utils;
using Microsoft.Xna.Framework;
using NLua;
using NLua.Exceptions;
using PipelineExtensionLibrary;

namespace Core.Scripting;

public class ScriptLoader
{
    private readonly BattleRegistry _battleRegistry;
    private readonly Dictionary<NamespacedKey, NamespacedDataStore> _dataStores = new();

    private readonly LuaFriendlyParticipantsProvider _friendlyParticipantsProvider = new();
    private readonly IGameSave _gameSave;
    private readonly ILocalizationManager _localizationManager;
    private readonly List<Lua> _runtimes = new();
    private readonly StateRegistry _stateRegistry;
    private readonly WeakList<NamespacedDataStore> _stores = new();
    private Color _defaultBackgroundColor = new(18, 14, 18);

    public ScriptLoader(StateRegistry stateRegistry, BattleRegistry battleRegistry, IGameSave gameSave,
        ILocalizationManager localizationManager)
    {
        _stateRegistry = stateRegistry;
        _battleRegistry = battleRegistry;
        _gameSave = gameSave;
        _localizationManager = localizationManager;
        _battleRegistry.FriendlyParticipantsProvider = _friendlyParticipantsProvider;
        var lua = new Lua();
        _runtimes.Add(lua);
        lua.DoString("function createSandbox() " + LuaSandbox.Sandbox + " end");
    }

    public void LoadScript(string script, ScriptContext context)
    {
        var lua = _runtimes[0];

        #region Create import functions

        DataStoreReader Import(string modId, string path = null)
        {
            if (path == null)
            {
                path = modId;
                modId = context.GetModId();
            }

            var key = new NamespacedKey(modId, path);
            var dataStore = _dataStores.GetOrCreate(key, () => new LuaNamespacedDataStore(key, lua, _gameSave));
            return new DataStoreReader(dataStore);
        }

        #endregion

        #region Setup data store

        var dataStore = _dataStores.GetOrCreate(context.GetName(),
            () => new LuaNamespacedDataStore(context.GetName(), lua, _gameSave));
        _stores.Add(dataStore);

        void SetDataSaver(LuaFunction saveFunction)
        {
            dataStore.SetDataSaveFunction(new LuaDataSaveFunction(saveFunction));
        }

        void SetDataLoader(LuaFunction loadFunction)
        {
            dataStore.SetDataLoadFunction(new LuaDataLoadFunction(loadFunction));
        }

        #endregion

        #region Exposed Lua Interfaces

        lua["GetTranslation"] = GetTranslation;
        lua["CreateStatusEffect"] = CreateEffectFactoryBuilder;
        lua["CreateAbility"] = CreateAbilityFactoryBuilder;
        lua["CreateConsumableHandler"] = CreateConsumableHandler;
        lua["CreateConstantAbility"] = CreateConstantAbilityFactoryBuilder;
        lua["CreateParticipant"] = CreateParticipantFactoryBuilder;
        lua["StateBuilder"] = BuildState;
        lua["SetDefaultBackgroundColor"] = SetDefaultBackgroundColor;
        lua["SetEntryState"] = SetEntryState;
        lua["BattleAction"] = new BattleActionsLuaBridge();
        lua["RegisterFriendlyParticipantsProvider"] =
            _friendlyParticipantsProvider.RegisterFriendlyParticipantsProvider;
        lua["Global"] = _stateRegistry.GlobalEventHandler;
        lua["Import"] = Import;
        lua["Context"] = new DataStoreWriter(dataStore);
        lua["SetDataSaver"] = SetDataSaver;
        lua["SetDataSaver"] = SetDataSaver;
        lua["SetDataLoader"] = SetDataLoader;

        #endregion

        try
        {
            (((lua["createSandbox"] as LuaFunction)!.Call().First() as LuaTable)!["run"] as LuaFunction)!
                .Call(script);
        }

        #region Format Error

        catch (LuaScriptException e)
        {
            var parts = e.Message.Split(':');
            if (parts.Length == 1) parts = e.Source.Split(':');
            throw new LuaModException(context, int.Parse(parts[parts.Length - 2]), e);
        }

        #endregion


        // Update default color of null state
        if (_stateRegistry.ReadState("null") is NullState nullState) nullState.SetBackground(_defaultBackgroundColor);
    }

    public void SetEntryState(string state)
    {
        _stateRegistry.EntryState = state;
    }

    public WrappedTranslation GetTranslation(string key, LuaTable replacements = null)
    {
        return new WrappedTranslation(_localizationManager.GetData(key, LuaUtils.ReadReplacements(replacements)));
    }

    ~ScriptLoader()
    {
        _runtimes.ForEach(run => run.Dispose());
    }

    internal LuaStateBuilder BuildState(string stateId)
    {
        return new LuaStateBuilder(stateId, _defaultBackgroundColor, _stateRegistry.RegisterState);
    }

    internal void SetDefaultBackgroundColor(string color)
    {
        _defaultBackgroundColor = color.ToColor();
    }

    private LuaEffectFactoryBuilder CreateEffectFactoryBuilder(string id)
    {
        return new LuaEffectFactoryBuilder(id, factory => _battleRegistry.RegisterEffect(factory));
    }

    private LuaAbilityFactoryBuilder CreateAbilityFactoryBuilder(string id)
    {
        return new LuaAbilityFactoryBuilder(id, factory => _battleRegistry.RegisterAbility(factory));
    }

    private LuaAbilityBuilder CreateConstantAbilityFactoryBuilder(string id)
    {
        return new LuaAbilityBuilder(id, ability =>
            _battleRegistry.RegisterAbility(new ConstantLuaAbilityFactory(id, ability)));
    }

    private void CreateConsumableHandler(LuaFunction function)
    {
        _battleRegistry.RegisterConsumableProvider(new LuaConsumableProvider(function));
    }

    private LuaParticipantFactoryBuilder CreateParticipantFactoryBuilder(string id)
    {
        return new LuaParticipantFactoryBuilder(id, factory => _battleRegistry.RegisterParticipant(factory));
    }

    /// <summary>
    ///     Runs all load hooks. Should be called after all scripts have been loaded
    /// </summary>
    public void Load()
    {
        foreach (var store in _stores) store.Load();
    }

    public void Save()
    {
        foreach (var dataStore in _dataStores.Values) dataStore.Save();
    }
}