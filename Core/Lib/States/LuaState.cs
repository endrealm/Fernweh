using System;
using NLua;

namespace Core.States;

public class LuaState: IState
{
    private readonly LuaFunction _renderFunc;

    public LuaState(string id, LuaFunction renderFunc, bool showExit)
    {
        Id = id;
        _renderFunc = renderFunc;
        ShowExit = showExit;
    }

    public string Id { get; }

    public void Render(StateRenderer renderer, RenderContext context)
    {
        _renderFunc.Call(renderer, context);
    }

    public bool ShowExit { get; }
}