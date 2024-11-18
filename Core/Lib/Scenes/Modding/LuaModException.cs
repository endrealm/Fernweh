using System;
using Core.Scripting;
using NLua.Exceptions;

namespace Core.Scenes.Modding;

public class LuaModException : Exception
{
    private readonly ScriptContext _context;
    private readonly LuaException _exception;
    private readonly int _line;

    public LuaModException(ScriptContext context, int line, LuaException exception)
    {
        _context = context;
        _line = line;
        _exception = exception;
    }

    public override string Message => $"File {_context.GetName().Pretty()}.lua#{_line} -> " + _exception.Message;
    public override string StackTrace => _exception.StackTrace;

    public override Exception GetBaseException()
    {
        return _exception;
    }
}