using Core.States;

namespace Core.Scenes.Ingame.Views;

public class StateChatView: BaseChatView
{
    public void RenderResults(StateRenderer renderer)
    {
        Width = 0; // reset width so rescale is triggered
        QueuedComponents = renderer.Build();
        RunningComponents.Clear();
        LoadNextComponentInQueue();
    }
}