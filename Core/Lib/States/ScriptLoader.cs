using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl;
using Microsoft.Xna.Framework;
using NLua;
using PipelineExtensionLibrary;
using Lua = NLua.Lua;

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
        lua["CreateStatusEffect"] = CreateEffectFactoryBuilder;
        lua["stateBuilder"] = BuildState;
        lua["setDefaultBackgroundColor"] = SetDefaultBackgroundColor;
        lua["global"] = _stateRegistry.GlobalEventHandler;
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
        return new LuaEffectFactoryBuilder(id, factory => _battleRegistry.Register(factory));
    }
    
}