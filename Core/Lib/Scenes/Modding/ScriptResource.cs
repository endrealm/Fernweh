using Core.Content;
using Core.Scripting;

namespace Core.Scenes.Modding;

public class ScriptResource : IResource
{
    private readonly IArchiveLoader _loader;
    private readonly Namespace _parent;
    private readonly string _path;
    private string _script;

    public ScriptResource(Namespace parent, string path, IArchiveLoader loader)
    {
        _parent = parent;
        _path = path;
        _loader = loader;
    }

    public void Load()
    {
        _script = _loader.LoadFile(_path + ".lua");
    }

    public void Clear()
    {
        _script = null;
    }

    public void Apply(Mod mod, ScriptLoader scriptLoader)
    {
        scriptLoader.LoadScript(_script, BuildContext(mod));
    }

    private ScriptContext BuildContext(Mod mod)
    {
        return new ScriptContext(new NamespacedKey(mod.Id, _path), mod.Id);
    }
}