namespace Core.Scripting.Saving;

public class VariableWrapper : IDataType
{
    private object _data;

    public VariableWrapper(string key, object data)
    {
        Key = key;
        _data = data;
    }

    public string Key { get; }

    public void Set(object data)
    {
        _data = data;
    }

    public object Get()
    {
        return _data;
    }
}