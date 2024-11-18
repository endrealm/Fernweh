using System.Linq;
using NLua;

namespace Core.Scripting.Saving;

public class LuaDataSaveFunction : IDataSaveFunction
{
    private readonly LuaFunction _function;

    public LuaDataSaveFunction(LuaFunction function)
    {
        _function = function;
    }

    public object Save(IDataEncoder dataEncoder)
    {
        if (_function == null) return null;
        var data = _function.Call().First();
        var encoded = dataEncoder.EncodeData(data);
        return encoded;
    }
}