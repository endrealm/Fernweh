using System;
using System.Collections.Generic;

namespace Core.States.ScriptApi;

public class Namespace
{

    private readonly string _name;
    private readonly INamespaceAccessPolicy _policy;

    private readonly Dictionary<string, object> _members = new();
    private readonly Dictionary<string, Namespace> _childNamespaces = new();

    public Namespace(string name, INamespaceAccessPolicy policy)
    {
        _name = name;
        _policy = policy;
    }
    

    public void AddNamespace(Namespace ns)
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

    public Namespace GetNamespace(string name)
    {
        if (!_childNamespaces.ContainsKey(name)) throw new Exception("Invalid namespace " + name);
        return _childNamespaces[name];
    }

    public Dictionary<string, object> GetMembers()
    {
        return _members;
    }

}