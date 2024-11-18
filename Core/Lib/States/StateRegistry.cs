using System;
using System.Collections.Generic;

namespace Core.States;

public class StateRegistry
{
    private readonly Dictionary<string, IState> _states = new()
    {
        {"null", new NullState()}
    };

    public readonly IGlobalEventHandler GlobalEventHandler = new LuaGlobalEventHandler();

    public string EntryState { get; set; } = "null";

    public IState ReadState(string stateId)
    {
        if (!_states.TryGetValue(stateId, out var state))
        {
            Console.WriteLine("StateRegistry WARN: Unknown state " + stateId + " using null state instead!");
            return _states["null"];
        }

        return state;
    }

    public void RegisterState(IState state)
    {
        _states.Add(state.Id, state);
    }
}