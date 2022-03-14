namespace Core.States;

public interface IState
{
    string Id { get; }
    void Render(StateRenderer renderer, RenderContext context);
    bool ShowExit { get; }
    bool AllowMove { get; }
}