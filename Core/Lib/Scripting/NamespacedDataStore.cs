using System.Collections.Generic;

namespace Core.Scripting;

public class NamespacedDataStore
{
    public NamespacedKey Key { get; }
    private Dictionary<string, IDataType> _data = new();

    public NamespacedDataStore(NamespacedKey key)
    {
        Key = key;
    }

    public VariableWrapper CreateVar(string key, object defaultValue = null)
    {
        var variable = new VariableWrapper(defaultValue);
        _data.Add(key, variable);
        variable.Set(defaultValue);
        return variable;
    }

    public IDataType GetData(string key)
    {
        return _data.TryGetValue(key, out var value) ? value : null;
    }
}

public interface IDataType
{
    
}

public class VariableWrapper: IDataType
{
    private object _data;

    public VariableWrapper(object data)
    {
        _data = data;
    }

    public void Set(object data)
    {
        _data = data;
    }
    
    public object Get()
    {
        return _data;
    }
}