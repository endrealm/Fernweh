using System;
using System.Collections.Generic;
using Lua = NLua.Lua;

namespace Core.States;

public class StateManager
{
    private Dictionary<string, IState> _states = new();
    public void LoadScript(string script)
    {
        using (Lua lua = new Lua())
        {
            lua["stateBuilder"] = BuildState;
            lua.DoString(script);
        }
    }

    private LuaStateBuilder BuildState(string stateId)
    {
        return new LuaStateBuilder(stateId, state => RegisterState(state));
    }

    private void RegisterState(IState state)
    {
        _states.Add(state.Id, state);
    }
}