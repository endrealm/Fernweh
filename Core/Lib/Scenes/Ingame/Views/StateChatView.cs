using Core.Scenes.Ingame.Localization;
using Core.States;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Views;

public class StateChatView: BaseChatView
{
    public void RenderResults(StateRenderer renderer, bool sticky)
    {
        Clear();
        SetSticky(sticky);
        QueuedComponents = renderer.Build();
        LoadNextComponentInQueue();
    }

    public StateChatView(ILocalizationManager localizationManager, IFontManager fontManager) : base(localizationManager, fontManager)
    {
    }
}