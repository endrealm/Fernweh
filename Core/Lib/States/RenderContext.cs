using Core.Scenes.Ingame;
// ReSharper disable InconsistentNaming

namespace Core.States;

public class RenderContext
{
    private readonly GameManager _gameManager;

    public RenderContext(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void changeState(string stateId)
    {
        _gameManager.LoadState(stateId);
    }
    public void exit()
    {
        _gameManager.LoadState("null");
    }
}