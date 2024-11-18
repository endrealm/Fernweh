using NLua;

namespace Core.Scripting.Saving;

public class LuaDataLoadFunction : IDataLoadFunction
{
    private readonly LuaFunction _function;

    public LuaDataLoadFunction(LuaFunction function)
    {
        _function = function;
    }

    public void Load(IDataEncoder dataEncoder, object data)
    {
        if (_function == null) return;
        var encoded = dataEncoder.DecodeData(data);
        _function.Call(encoded);
    }
}