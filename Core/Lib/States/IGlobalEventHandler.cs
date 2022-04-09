namespace Core.States;

public interface IGlobalEventHandler
{
    void EmitPreStateChangeEvent();
    void EmitPrePlayerMoveEvent();
    void EmitPostStateChangeEvent(StateRenderer renderer, RenderContext renderContext);
}