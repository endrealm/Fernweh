namespace Core.Scripting.Saving;


public class VariableWrapper: IDataType
{
    private readonly string _key;
    private object _data;

    public VariableWrapper(string key, object data)
    {
        _key = key;
        _data = data;
    }

    public string Key => _key;

    public void Set(object data)
    {
        _data = data;
    }
    
    public object Get()
    {
        return _data;
    }
}