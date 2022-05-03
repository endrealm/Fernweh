using Core.Scenes.Ingame.Localization;
using Core.States;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Views;

public class StateChatView: BaseChatView
{
    public void RenderResults(StateRenderer renderer, bool sticky)
    {
        if (renderer.ClearRender)
        {
            Clear();
        }
        SetSticky(sticky);
        renderer.GetLabelSettings().ForEach(settings => DrawLabel(settings));
        QueuedComponents = renderer.Build();
        LoadNextComponentInQueue();
    }

    public StateChatView(ILocalizationManager localizationManager, IFontManager fontManager) : base(localizationManager, fontManager)
    {
    }
}