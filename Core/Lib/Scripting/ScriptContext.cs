namespace Core.Scripting;

public class ScriptContext
{

    private readonly NamespacedKey _name;
    private readonly string _modId;

    public ScriptContext(NamespacedKey name, string modId)
    {
        _name = name;
        _modId = modId;
    }

    public NamespacedKey GetName()
    {
        return _name;
    }

    public string GetModId()
    {
        return _modId;
    }
    
}

public struct NamespacedKey
{
    public string Key { get; }
    public string Value { get; }

    public NamespacedKey(string key, string value)
    {
        Key = key;
        Value = value;
    }

}