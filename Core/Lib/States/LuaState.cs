﻿using System;
using Microsoft.Xna.Framework;
using NLua;

namespace Core.States;

public class LuaState: IState
{
    private readonly LuaFunction _renderFunc;
    private readonly Color _backgroundColor;

    public LuaState(string id, LuaFunction renderFunc, bool showExit, bool allowMove, Color backgroundColor)
    {
        Id = id;
        _renderFunc = renderFunc;
        _backgroundColor = backgroundColor;
        ShowExit = showExit;
        AllowMove = allowMove;
    }

    public string Id { get; }

    public void Render(StateRenderer renderer, RenderContext context)
    {
        renderer.SetBackgroundColor(_backgroundColor);
        _renderFunc.Call(renderer, context);
    }

    public bool ShowExit { get; }
    public bool AllowMove { get; }
}