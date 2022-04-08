using System.Collections.Generic;
using Core.Content;
using Core.Scripting;
using Core.States;

namespace Core.Scenes.Modding;

public class Mod
{
    private readonly ModIndex _index;
    private readonly IArchiveLoader _loader;
    private Namespace _root;
    private List<ScriptResource> _scripts;
    public Mod(ModIndex index, IArchiveLoader loader)
    {
        _index = index;
        _loader = loader;
    }

    public string Id => _index.Id;
    public bool IsLoaded => _root != null;
    public string[] Dependencies => _index.Dependencies;

    public void Load()
    {
        _root = new Namespace(Id, null);
        _scripts = new List<ScriptResource>();
        
        foreach (var path in _index.Scripts)
        {
            var current = _root;
            var pieces = path.Split('/');

            for (var i = 0; i < pieces.Length; i++)
            {
                if (i == pieces.Length-1)
                {
                    var script = new ScriptResource(current, path.Replace(".lua", ""), _loader);
                    current.AddResource(script);
                    _scripts.Add(script);
                    continue;
                }

                current = current.GetOrCreateNamespace(pieces[i]);
            }
        }
    }

    /// <summary>
    /// Requires "Load" to be called first
    /// </summary>
    /// <param name="scriptLoader"></param>
    public void Apply(ScriptLoader scriptLoader)
    {
        _scripts.ForEach(script =>
        {
            script.Load();
            script.Apply(this, scriptLoader);
        });
    }
}