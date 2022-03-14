using System;
using System.Collections.Generic;
using System.Linq;
using NLua;
using Lua = NLua.Lua;

namespace Core.States;

public class StateManager
{
    private readonly Dictionary<string, IState> _states = new();
    private readonly List<Lua> _runtimes = new();
    public void LoadScript(string script)
    {
        var lua = new Lua();
        lua["stateBuilder"] = BuildState;
        lua.DoString("function createSandbox() " + LuaSandbox.SANDBOX + " end");
        (((lua["createSandbox"] as LuaFunction).Call().First() as LuaTable)["run"] as LuaFunction)
            .Call(script);
        _runtimes.Add(lua);
    }
    
    ~StateManager() {
        _runtimes.ForEach(run => run.Dispose());
    }
    
    public IState ReadState(string stateId)
    {
        return _states[stateId];
    }

    private LuaStateBuilder BuildState(string stateId)
    {
        return new LuaStateBuilder(stateId, RegisterState);
    }

    private void RegisterState(IState state)
    {
        _states.Add(state.Id, state);
    }
}