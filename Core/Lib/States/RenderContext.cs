using Core.Scenes.Ingame;

namespace Core.States;

public class RenderContext
{
    private readonly GameManager _gameManager;

    public RenderContext(GameManager gameManager)
    {
        _gameManager = gameManager;
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