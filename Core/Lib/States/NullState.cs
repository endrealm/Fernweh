namespace Core.States;

public class NullState: IState
{
    public string Id => "null";
    public void Render(StateRenderer renderer, RenderContext context)
    {
        // Do nothing
    }

    public bool ShowExit => false;
}