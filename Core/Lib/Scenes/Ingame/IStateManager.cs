using Core.States;

namespace Core.Scenes.Ingame;

public interface IStateManager
{
    public IState ActiveState { get; }
    public string weakNextID { get; set; }
    public void LoadState(string stateId);
}