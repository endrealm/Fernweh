using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl;
using Microsoft.Xna.Framework;
using NLua;
using PipelineExtensionLibrary;

namespace Core.States;

public class ScriptLoader
{
    private readonly List<Lua> _runtimes = new();
    private readonly StateRegistry _stateRegistry;
    private readonly BattleRegistry _battleRegistry;
    private Color _defaultBackgroundColor = new(18, 14, 18);

    public ScriptLoader(StateRegistry stateRegistry, BattleRegistry battleRegistry)
    {
        _stateRegistry = stateRegistry;
        _battleRegistry = battleRegistry;
    }

    public void LoadScript(string script)
    {
        var lua = new Lua();

        #region Exposed Lua Interfaces

        lua["CreateStatusEffect"] = CreateEffectFactoryBuilder;
        lua["CreateAbility"] = CreateAbilityFactoryBuilder;
        lua["CreateConstantAbility"] = CreateConstantAbilityFactoryBuilder;
        lua["CreateParticipant"] = CreateParticipantFactoryBuilder;
        lua["StateBuilder"] = BuildState;
        lua["SetDefaultBackgroundColor"] = SetDefaultBackgroundColor;
        lua["Global"] = _stateRegistry.GlobalEventHandler;

        #endregion
       
        
        lua.DoString("function createSandbox() " + LuaSandbox.Sandbox + " end");
        (((lua["createSandbox"] as LuaFunction)!.Call().First() as LuaTable)!["run"] as LuaFunction)!
            .Call(script);
        _runtimes.Add(lua);

        if (_stateRegistry.ReadState("null") is NullState nullState)
        {
            nullState.SetBackground(_defaultBackgroundColor);
        }
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
    
}