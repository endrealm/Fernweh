using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Scenes.Modding;

public class DependencyGraph
{
    private List<DependencyNode> _roots = new List<DependencyNode>();

    public DependencyNode GetOrAdd(string id)
    {
        foreach (var root in _roots)
        {
            var result = root.Find(id);
            if (result != null) return result;
        }

        var node = new DependencyNode(id, this);
        _roots.Add(node);
        return node;
    }
    
    public DependencyNode Get(string id)
    {
        foreach (var root in _roots)
        {
            var result = root.Find(id);
            if (result != null) return result;
        }
        return null;
    }

    public void MergeDangling()
    {
        var mergeable = new HashSet<int>();
        for (var i = 0; i < _roots.Count; i++)
        {
            var thisNode = _roots[i];
            for (var j = i+1; j < _roots.Count; j++)
            {
                if (thisNode.Find(_roots[j].Id) == null) continue;
                mergeable.Add(j);
            }
        }

        var nodesToMerge = mergeable.Select(index => _roots[index]).ToList();
        _roots.RemoveAll(node => nodesToMerge.Contains(node));
    }
}

public class DependencyNode
{
    private readonly DependencyGraph _graph;

    public DependencyNode(string id, DependencyGraph graph)
    {
        _graph = graph;
        Id = id;
    }

    public string Id { get; }
    public HashSet<DependencyNode> Dependents { get; } = new();
    public HashSet<DependencyNode> Dependencies { get; } = new();

    public void AddDependency(string dependency)
    {
        if(dependency == Id) return;

        var otherTree = false;
        var node = Find(dependency);
        
        if (node == null)
        {
            node = _graph.Get(dependency);
            if(node != null) otherTree = true;
        }
        node ??= new DependencyNode(dependency, _graph);
        
        Dependencies.Add(node);
        node.Dependents.Add(this);
        
        // Attached node from another tree so we might be able to merge
        if (otherTree)
        {
            _graph.MergeDangling();
        }
    }

    public DependencyNode Find(string id)
    {
        return FindInternal(id, new HashSet<string>());
    }
    
    private DependencyNode FindInternal(string id, HashSet<string> visited)
    {
        if (Id == id) return this;
        if (visited.Contains(Id)) return null;
        visited.Add(Id);
        foreach (var node in Dependencies)
        {
            var result = node.FindInternal(id, visited);
            if (result != null) return result;
        }
        foreach (var node in Dependents)
        {
            var result = node.FindInternal(id, visited);
            if (result != null) return result;
        }
        return null;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public List<DependencyNode> CollectFlatHierarchy()
    {
        return CollectFlatHierarchyInternal(new HashSet<string>(), new HashSet<string>());
    }

    private List<DependencyNode> CollectFlatHierarchyInternal(HashSet<string> visited, HashSet<string> parents)
    {
        if (parents.Contains(Id)) throw new Exception("Cyclic mod dependency detected");
        if (visited.Contains(Id)) return new List<DependencyNode>();

        var list = new List<DependencyNode>();
        var parentSet = new HashSet<string>(parents) {Id};

        foreach (var dependency in Dependencies)
        {
            list.AddRange(dependency.CollectFlatHierarchyInternal(visited, parentSet));
        }
        
        list.Add(this);
        return list;
    }
}