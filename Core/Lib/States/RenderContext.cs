using Core.Scenes.Ingame;

namespace Core.States;

public class RenderContext
{
    private readonly IStateManager _gameManager;

    public RenderContext(IStateManager stateManager)
    {
        _gameManager = stateManager;
    }

    public void ChangeState(string stateId)
    {
        _gameManager.LoadState(stateId);
    }
    public void Exit()
    {
        _gameManager.LoadState("null");
    }
}