using Microsoft.Xna.Framework;
using NLua;

namespace Core.States;

public class LuaState: IState
{
    private readonly LuaFunction _renderFunc;
    private readonly Color _backgroundColor;

    public LuaState(string id, LuaFunction renderFunc, bool showExit, bool allowMove, Color backgroundColor,
        bool allowSave, bool sticky)
    {
        Id = id;
        _renderFunc = renderFunc;
        _backgroundColor = backgroundColor;
        ShowExit = showExit;
        AllowMove = allowMove;
        AllowSave = allowSave;
        Sticky = sticky;
    }

    public string Id { get; }

    public void Render(StateRenderer renderer, RenderContext context)
    {
        renderer.SetBackgroundColor(_backgroundColor);
        _renderFunc.Call(renderer, context);
    }

    public bool ShowExit { get; }
    public bool AllowMove { get; }
    public bool AllowSave { get; }
    public bool Sticky { get; }
}