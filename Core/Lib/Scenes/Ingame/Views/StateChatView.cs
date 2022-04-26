using Core.Scenes.Ingame.Localization;
using Core.States;
using PipelineExtensionLibrary;

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

    public StateChatView(ILocalizationManager localizationManager, IFontManager fontManager) : base(localizationManager, fontManager)
    {
    }
}