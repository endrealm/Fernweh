namespace Core.States.ScriptApi;

public class ScriptContext
{

    private readonly string _name;
    private readonly string _modId;

    public ScriptContext(string name, string modId)
    {
        _name = name;
        _modId = modId;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetModId()
    {
        return _modId;
    }
    
}