using Core.States;

namespace Core.Scenes.Ingame;

public interface IStateManager
{
    public IState ActiveState { get; }
    public void LoadState(string stateId);
}