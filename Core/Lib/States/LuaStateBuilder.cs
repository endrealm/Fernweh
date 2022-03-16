using System;
using Microsoft.Xna.Framework;
using NLua;
using PipelineExtensionLibrary;

namespace Core.States;

public class LuaStateBuilder
{
    private readonly string _stateId;
    private Color _defaultBackgroundColor;
    private readonly Action<IState> _onStateBuild;
    private LuaFunction _renderFunc;
    private bool _showExit;
    private bool _allowMove;

    public LuaStateBuilder(string stateId, Color defaultBackgroundColor, Action<IState> onStateBuild)
    {
        _stateId = stateId;
        _defaultBackgroundColor = defaultBackgroundColor;
        _onStateBuild = onStateBuild;
    }

    public LuaStateBuilder Render(LuaFunction renderFunc)
    {
        _renderFunc = renderFunc;
        return this;
    }
    public LuaStateBuilder ShowExit(bool showExit)
    {
        _showExit = showExit;
        return this;
    }
    public LuaStateBuilder AllowMove(bool allowMove)
    {
        _allowMove = allowMove;
        return this;
    }
    
    public LuaStateBuilder BackgroundColor(string color)
    {
        _defaultBackgroundColor = color.ToColor();
        return this;
    }

    public IState Build()
    {
        var state = new LuaState(_stateId, _renderFunc, _showExit, _allowMove, _defaultBackgroundColor);
        _onStateBuild.Invoke(state);
        return state;
    }
}