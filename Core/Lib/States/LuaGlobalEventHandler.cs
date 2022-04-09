using System.Collections.Generic;
using NLua;

namespace Core.States;

public class LuaGlobalEventHandler: IGlobalEventHandler
{
    private readonly List<LuaFunction> _preStateChangeEventListeners = new();
    private readonly List<LuaFunction> _postStateChangeEventListeners = new();
    private readonly List<LuaFunction> _prePlayerMoveEventListeners = new();

    public LuaGlobalEventHandler AddOnPrePlayerMove(LuaFunction listener)
    {
        _prePlayerMoveEventListeners.Add(listener);
        return this;
    }
    public LuaGlobalEventHandler AddOnPreStateRender(LuaFunction listener)
    {
        _preStateChangeEventListeners.Add(listener);
        return this;
    }
    
    public LuaGlobalEventHandler AddOnPostStateRender(LuaFunction listener)
    {
        _postStateChangeEventListeners.Add(listener);
        return this;
    }

    public void EmitPreStateChangeEvent()
    {
        _preStateChangeEventListeners.ForEach(fun => fun.Call());
    }
    public void EmitPostStateChangeEvent(StateRenderer renderer, RenderContext renderContext)
    {
        _postStateChangeEventListeners.ForEach(fun => fun.Call(renderer, renderContext));
    }

    public void EmitPrePlayerMoveEvent()
    {
        _prePlayerMoveEventListeners.ForEach(fun => fun.Call());
    }
}