using System;
using NLua;

namespace Core.States;

public class LuaStateBuilder
{
    private readonly string _stateId;
    private readonly Action<IState> _onStateBuild;
    private LuaFunction _renderFunc;
    private bool _showExit;

    public LuaStateBuilder(string stateId, Action<IState> onStateBuild)
    {
        _stateId = stateId;
        _onStateBuild = onStateBuild;
    }

    public LuaStateBuilder render(LuaFunction renderFunc)
    {
        _renderFunc = renderFunc;
        return this;
    }
    public LuaStateBuilder showExit(bool showExit)
    {
        _showExit = showExit;
        return this;
    }

    public IState build()
    {
        var state = new LuaState(_stateId, _renderFunc, _showExit);
        _onStateBuild.Invoke(state);
        return state;
    }
}