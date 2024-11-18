using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Views;
using Core.States;

namespace Core.Scenes.Ingame.Modes.Overworld;

public class StateChatView : BaseChatView
{
    public StateChatView(ILocalizationManager localizationManager, IFontManager fontManager) : base(localizationManager,
        fontManager)
    {
    }

    public void RenderResults(StateRenderer renderer, bool sticky)
    {
        if (renderer.ClearRender) Clear();
        SetSticky(sticky);
        renderer.GetLabelSettings().ForEach(settings => DrawLabel(settings));
        QueuedComponents = renderer.Build();
        LoadNextComponentInQueue();
    }
}