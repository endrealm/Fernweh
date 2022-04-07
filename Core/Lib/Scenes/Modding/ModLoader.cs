using System.Collections.Generic;
using System.Linq;
using Core.Content;
using Core.Content.Mod;
using Core.Scripting;
using Core.States;
using Newtonsoft.Json;

namespace Core.Scenes.Modding;

public class ModLoader
{
    private readonly Dictionary<string, Mod> _mods = new();

    public ModLoader(List<IArchiveLoader> mods)
    {
        foreach (var ns in mods.Select(LoadMod))
        {
            _mods.Add(ns.Id, ns);
        }
    }

    private Mod LoadMod(IArchiveLoader loader)
    {
        var index = JsonConvert.DeserializeObject<ModIndex>(loader.LoadFile("index.json"));
        return new Mod(index, loader);
    }

    public void Load(ScriptLoader scriptLoader, string gameMod)
    {
        var loadMods = BuildOrder(gameMod);
        
        foreach (var mod in loadMods)
        {
            mod.Load();
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
}