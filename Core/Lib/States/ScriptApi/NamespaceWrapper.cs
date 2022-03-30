namespace Core.States.ScriptApi;

public class NamespaceWrapper
{

    private Namespace _namespace;

    public NamespaceWrapper(Namespace ns)
    {
        _namespace = ns;
    }

    public INamespaceAccess Access(ScriptContext context)
    {
        return _namespace.Access(context);
    }

}