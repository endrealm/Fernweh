using System;
using System.Collections.Generic;

namespace Core.States.ScriptApi;

public interface INamespaceAccess
{

    public NamespaceBuilder CreateNamespace(string name);

    public Dictionary<string, object> All();
    public void Set(string member, object value);
    public object Get(string member);

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
        if (!_isWrite) throw new Exception("No permission to create new namespace");
        return new NamespaceBuilder(name, _namespace.AddNamespace);
    }

    public NamespaceWrapper GetNamespace(string name)
    {
        if (!_isRead) throw new Exception("No permission to get namespace " + name);
        return new NamespaceWrapper(_namespace.GetNamespace(name));
    }

    public Dictionary<string, object> All()
    {
        if (!_isRead || !_isWrite) throw new Exception("No permission to read members");
        return _namespace.GetMembers();
    }

    public void Set(string member, object value)
    {
        if (!_isWrite) throw new Exception("No permission to write member");
        _namespace.GetMembers()[member] = value;
        Console.WriteLine("set: " + member + " " + (member == null));
    }
    
    public object Get(string member)
    {
        if (!_isRead) throw new Exception("No permission to read member");
        var result = _namespace.GetMembers()[member];
        Console.WriteLine("get: " + member + " " + result);
        return _namespace.GetMembers()[member];
    }

}