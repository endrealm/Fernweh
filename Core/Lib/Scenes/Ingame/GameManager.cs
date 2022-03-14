using System;
using Core.States;

namespace Core.Scenes.Ingame;

public class GameManager
{
    private readonly StateRegistry _stateRegistry;
    public event StateChangedEventHandler StateChangedEvent;
    public IState ActiveState { get; private set; }

    public GameManager(StateRegistry stateRegistry)
    {
        _stateRegistry = stateRegistry;
        ActiveState = stateRegistry.ReadState("null"); // Start with "null" state.
    }

    public void LoadState(string stateId)
    {
        ActiveState = _stateRegistry.ReadState(stateId);
        StateChangedEvent?.Invoke(new StateChangedEventArgs()
        {
            NewState = ActiveState
        });
    }
}

public delegate void StateChangedEventHandler(StateChangedEventArgs args);

public class StateChangedEventArgs : EventArgs
{
    public IState NewState { get; set; }
}
