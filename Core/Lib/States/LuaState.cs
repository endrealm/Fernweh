using System;
using NLua;

namespace Core.States;

public class LuaState: IState
{
    private readonly LuaFunction _renderFunc;

    public LuaState(string id, LuaFunction renderFunc, bool showExit, bool allowMove)
    {
        Id = id;
        _renderFunc = renderFunc;
        ShowExit = showExit;
        AllowMove = allowMove;
    }

    public string Id { get; }

    public void Render(StateRenderer renderer, RenderContext context)
    {
        _renderFunc.Call(renderer, context);
    }

    public bool ShowExit { get; }
    public bool AllowMove { get; }
}