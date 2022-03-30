namespace Core.States.ScriptApi;

public interface INamespaceAccessPolicy
{

    INamespaceAccess Access(ScriptContext context, Namespace ns);

}


public class SimpleNamespaceAccessPolicy : INamespaceAccessPolicy
{
    public INamespaceAccess Access(ScriptContext context, Namespace ns)
    {
        return new SimpleNamespaceAccess(true, true, ns);
    }
    
}