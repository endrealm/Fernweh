﻿using System;

namespace Core.States.ScriptApi;

public class NamespaceBuilder
{

    private readonly Action<Namespace> _register;
    private readonly string _name;

    private INamespaceAccessPolicy _policy = new SimpleNamespaceAccessPolicy();


    public NamespaceBuilder(string name, Action<Namespace> register)
    {
        _name = name;
        _register = register;
    }

    public NamespaceBuilder SetPolicy(INamespaceAccessPolicy policy)
    {
        _policy = policy;
        return this;
    }
    
    public INamespaceAccess Build()
    {
        var newNamespace = new Namespace(_name, _policy);
        _register.Invoke(newNamespace);
        return new SimpleNamespaceAccess(true, true);
    }

}