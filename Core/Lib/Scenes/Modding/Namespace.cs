using System.Collections.Generic;

namespace Core.Scenes.Modding;

public class Namespace
{
    private readonly Dictionary<string, Namespace> _children = new();
    private readonly Namespace _parent;
    private readonly List<IResource> _resources = new();

    public Namespace(string id, Namespace parent)
    {
        _parent = parent;
        Id = id;
    }

    public string Id { get; }

    public void AddResource(IResource resource)
    {
        _resources.Add(resource);
    }

    public Namespace GetOrCreateNamespace(string id)
    {
        if (_children.ContainsKey(id)) return _children[id];

        var newSpace = new Namespace(id, this);
        _children.Add(id, newSpace);
        return newSpace;
    }

    public void Unload()
    {
        foreach (var childrenValue in _children.Values) childrenValue.Unload();

        foreach (var resource in _resources) resource.Clear();
    }
}