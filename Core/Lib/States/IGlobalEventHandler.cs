namespace Core.States;

public interface IGlobalEventHandler
{
    void EmitPreStateChangeEvent();
    void EmitPrePlayerMoveEvent();
}