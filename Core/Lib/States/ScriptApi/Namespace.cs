using System;
using System.Collections.Generic;

namespace Core.States.ScriptApi;

public class Namespace
{

    private readonly string _name;
    private readonly INamespaceAccessPolicy _policy;

    private readonly Dictionary<string, Namespace> _childNamespaces = new();

    public Namespace(string name, INamespaceAccessPolicy policy)
    {
        _name = name;
        _policy = policy;
    }
    

    void AddNamespace(Namespace ns)
    {
        if (!_childNamespaces.ContainsKey(ns._name))
        {
            _childNamespaces[ns._name] = ns;
        }
        else
        {
            throw new Exception("Namespace " + ns._name + " already exists");
        }
    }


    public INamespaceAccess Access(ScriptContext context)
    {
        return _policy.Access(context, this);
    }
    
}




// ACCESS STUFF

public interface INamespaceAccess
{

    public NamespaceBuilder CreateNamespace(string name);

}


public class SimpleNamespaceAccess : INamespaceAccess
{

    private readonly bool _isWrite;
    private readonly bool _isRead;
    private readonly Namespace _namespace;

    public SimpleNamespaceAccess(bool isWrite, bool isRead, Namespace ns)
    {
        _isWrite = isWrite;
        _isRead = isRead;
        _namespace = ns;
    }
    
    public NamespaceBuilder CreateNamespace(string name)
    {
        return new NamespaceBuilder(name, _namespace.);
    }

}