using System.Collections.Generic;
using NLua;

namespace Core.States;

public class LuaGlobalEventHandler: IGlobalEventHandler
{
    private readonly List<LuaFunction> _preStateChangeEventListeners = new();
    private readonly List<LuaFunction> _prePlayerMoveEventListeners = new();

    public LuaGlobalEventHandler addOnPrePlayerMove(LuaFunction listener)
    {
        _prePlayerMoveEventListeners.Add(listener);
        return this;
    }
    public LuaGlobalEventHandler addOnPreStateRender(LuaFunction listener)
    {
        _preStateChangeEventListeners.Add(listener);
        return this;
    }

    public void EmitPreStateChangeEvent()
    {
        _preStateChangeEventListeners.ForEach(fun => fun.Call());
    }

    public void EmitPrePlayerMoveEvent()
    {
        _prePlayerMoveEventListeners.ForEach(fun => fun.Call());
    }
}