using System;
using System.Collections.Generic;
using System.Linq;
using Core.Saving;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using Core.Scenes.Modding;
using Core.States;
using Core.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;
using NLua;
using NLua.Exceptions;
using PipelineExtensionLibrary;

namespace Core.Scripting;

public class ScriptLoader
{
    private readonly List<Lua> _runtimes = new();
    private readonly Dictionary<NamespacedKey, NamespacedDataStore> _dataStores = new();
    private readonly StateRegistry _stateRegistry;
    private readonly BattleRegistry _battleRegistry;
    private readonly IGameSave _gameSave;

    private readonly LuaFriendlyParticipantsProvider _friendlyParticipantsProvider = new();
    private Color _defaultBackgroundColor = new(18, 14, 18);

    public ScriptLoader(StateRegistry stateRegistry, BattleRegistry battleRegistry, IGameSave gameSave)
    {
        _stateRegistry = stateRegistry;
        _battleRegistry = battleRegistry;
        _gameSave = gameSave;
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

        var dataStore = _dataStores.GetOrCreate(context.GetName(), () => new LuaNamespacedDataStore(context.GetName(), lua, _gameSave));
        
        #region Exposed Lua Interfaces

        lua["CreateStatusEffect"] = CreateEffectFactoryBuilder;
        lua["CreateAbility"] = CreateAbilityFactoryBuilder;
        lua["CreateConstantAbility"] = CreateConstantAbilityFactoryBuilder;
        lua["CreateParticipant"] = CreateParticipantFactoryBuilder;
        lua["StateBuilder"] = BuildState;
        lua["SetDefaultBackgroundColor"] = SetDefaultBackgroundColor;
        lua["SetEntryState"] = SetEntryState;
        lua["BattleAction"] = new BattleActionsLuaBridge();
        lua["RegisterFriendlyParticipantsProvider"] = _friendlyParticipantsProvider.RegisterFriendlyParticipantsProvider;
        lua["Global"] = _stateRegistry.GlobalEventHandler;
        lua["Import"] = Import;
        lua["Context"] = new DataStoreWriter(dataStore);

        #endregion
        try
        {
            (((lua["createSandbox"] as LuaFunction)!.Call().First() as LuaTable)!["run"] as LuaFunction)!
                .Call(script);
        }
        catch (LuaScriptException e)
        {
            var parts = e.Message.Split(':');
            if (parts.Length == 1)
            {
                parts = e.Source.Split(':');
            }
            throw new LuaModException(context, int.Parse(parts[parts.Length-2]), e);
        }

        if (_stateRegistry.ReadState("null") is NullState nullState)
        {
            nullState.SetBackground(_defaultBackgroundColor);
        }
    }

    public void SetEntryState(string state)
    {
        _stateRegistry.EntryState = state;
    }

    ~ScriptLoader() {
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

    private LuaParticipantFactoryBuilder CreateParticipantFactoryBuilder(string id)
    {
        return new LuaParticipantFactoryBuilder(id, factory => _battleRegistry.RegisterParticipant(factory));
    }

    public void Save()
    {
        foreach (var dataStore in _dataStores.Values)
        {
            dataStore.Save();
        }
    }
    
}