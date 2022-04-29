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
    private bool _allowSave = true;
    private bool _sticky = true;
    private bool _clearScreenPost = true;

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
    public LuaStateBuilder AllowSave(bool save)
    {
        _allowSave = save;
        return this;
    }
    public LuaStateBuilder Sticky(bool sticky)
    {
        _sticky = sticky;
        return this;
    }
    public LuaStateBuilder ClearScreenPost(bool clear)
    {
        _clearScreenPost = clear;
        return this;
    }
    
    public LuaStateBuilder BackgroundColor(string color)
    {
        _defaultBackgroundColor = color.ToColor();
        return this;
    }

    public IState Build()
    {
        var state = new LuaState(_stateId, _renderFunc, _showExit, _allowMove, _defaultBackgroundColor, _allowSave, _sticky, _clearScreenPost);
        _onStateBuild.Invoke(state);
        return state;
    }
}