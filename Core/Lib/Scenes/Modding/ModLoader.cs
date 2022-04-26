using System;
using System.Collections.Generic;
using System.Linq;
using Core.Content;
using Core.Scripting;
using Core.States;
using Newtonsoft.Json;

namespace Core.Scenes.Modding;

public class ModLoader
{
    private readonly Dictionary<string, Mod> _mods = new();
    private List<Mod> _activeModOrder = new();

    public ModLoader(List<IArchiveLoader> mods)
    {
        foreach (var ns in mods.Select(LoadMod))
        {
            _mods.Add(ns.Id, ns);
        }
    }

    public List<Mod> ActiveModOrder => _activeModOrder;

    private Mod LoadMod(IArchiveLoader loader)
    {
        var index = JsonConvert.DeserializeObject<ModIndex>(loader.LoadFile("index.json"));
        return new Mod(index, loader);
    }

    public void Load(string gameMod)
    {
        _activeModOrder = BuildOrder(gameMod);
        foreach (var mod in _activeModOrder)
        {
            mod.Load();
        }
    }

    public void RunActiveModScripts(ScriptLoader scriptLoader)
    {
        foreach (var mod in _activeModOrder)
        {
            mod.Apply(scriptLoader);
        }    
    }

    private List<Mod> BuildOrder(string gameMod)
    {
        var mods = _mods.Values;
        var graph = new DependencyGraph();

        foreach (var mod in mods)
        {
            var node = graph.GetOrAdd(mod.Id);
            foreach (var modDependency in mod.Dependencies)
            {
                node.AddDependency(modDependency);
            }
        }

        var modNode = graph.Get(gameMod);

        return modNode.CollectFlatHierarchy().Select(node => _mods[node.Id]).ToList();
    }

    public IArchiveLoader GetArchiveLoader(string modId)
    {
        return _mods.Values.First(mod => mod.Id == modId).Archive;
    }

    public void UnloadAllMods()
    {
        foreach (var mod in _mods.Values)
        {
            mod.Unload();
        }
    }

    public IEnumerable<Mod> GetGameMods()
    {
        return _mods.Values.Where(mod => mod.Type == ModType.Game);
    }
}