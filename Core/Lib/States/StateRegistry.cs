using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NLua;
using PipelineExtensionLibrary;
using Lua = NLua.Lua;

namespace Core.States;

public class StateRegistry
{
    public readonly IGlobalEventHandler GlobalEventHandler = new LuaGlobalEventHandler();
    private readonly Dictionary<string, IState> _states = new()
    {
        {"null", new NullState()}
    };
    private readonly List<Lua> _runtimes = new();
    private Color _defaultBackgroundColor = new(18, 14, 18);
    public void LoadScript(string script)
    {
        var lua = new Lua();
        lua["stateBuilder"] = BuildState;
        lua["setDefaultBackgroundColor"] = SetDefaultBackgroundColor;
        lua["global"] = GlobalEventHandler;
        lua.DoString("function createSandbox() " + LuaSandbox.Sandbox + " end");
        (((lua["createSandbox"] as LuaFunction)!.Call().First() as LuaTable)!["run"] as LuaFunction)!
            .Call(script);
        _runtimes.Add(lua);

        if (_states["null"] is NullState nullState)
        {
            nullState.SetBackground(_defaultBackgroundColor);
        }
    }
    
    ~StateRegistry() {
        _runtimes.ForEach(run => run.Dispose());
    }
    
    public IState ReadState(string stateId)
    {
        if (!_states.TryGetValue(stateId, out var state))
        {
            Console.WriteLine("StateRegistry WARN: Unknown state " + stateId +" using null state instead!");
            return _states["null"];
        }
        return state;
    }

    private LuaStateBuilder BuildState(string stateId)
    {
        return new LuaStateBuilder(stateId, _defaultBackgroundColor, RegisterState);
    }
    
    private void SetDefaultBackgroundColor(string color)
    {
        _defaultBackgroundColor = color.ToColor();
    }

    private void RegisterState(IState state)
    {
        _states.Add(state.Id, state);
    }
}