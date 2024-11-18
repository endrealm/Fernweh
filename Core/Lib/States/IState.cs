namespace Core.States;

public interface IState
{
    string Id { get; }
    bool ShowExit { get; }
    bool AllowMove { get; }
    bool AllowSave { get; }
    bool Sticky { get; }
    public bool ClearScreenPost { get; }
    void Render(StateRenderer renderer, RenderContext context);
}