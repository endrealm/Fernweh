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


    public IState ReadState(string stateId)
    {
        if (!_states.TryGetValue(stateId, out var state))
        {
            Console.WriteLine("StateRegistry WARN: Unknown state " + stateId +" using null state instead!");
            return _states["null"];
        }
        return state;
    }

    public void RegisterState(IState state)
    {
        _states.Add(state.Id, state);
    }
}