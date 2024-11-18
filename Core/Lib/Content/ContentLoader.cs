using System;
using System.Collections.Generic;
using Core.Scenes.Modding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Core.Content;

public enum ContentOrigin
{
    Core,
    Mod
}

public interface ILoader
{
}

public interface ILoader<out TResource> : ILoader
{
    public TResource Load(string file, IArchiveLoader archiveLoader);
}

public class ContentLoader
{
    private readonly ContentManager _contentManager;
    private readonly IFontManager _fontManager;

    private readonly Dictionary<Type, ILoader> _loaders = new();

    private readonly List<IArchiveLoader> _mods;

    public ContentLoader(GraphicsDeviceManager deviceManager, ContentManager contentManager, IFontManager fontManager,
        List<IArchiveLoader> mods)
    {
        _mods = mods;
        _contentManager = contentManager;
        _fontManager = fontManager;

        // Register default loaders
        RegisterLoader(new FileLoader());
        RegisterLoader(new TextureLoader(deviceManager));
        RegisterLoader(new DialogLoader(_contentManager));
        RegisterLoader(new LocaleLoader());
    }

    public ModLoader ModLoader { set; get; }

    public void LoadFonts()
    {
        _fontManager.Load(_contentManager);
    }

    public IFontManager GetFontManager()
    {
        return _fontManager;
    }

    public TResource Load<TResource>(string file, string modId = "core")
    {
        var loader = _loaders[typeof(TResource)];
        if (loader == null) throw new Exception("No loader specified for type");

        var archiveLoader = ModLoader.GetArchiveLoader(modId);

        return ((ILoader<TResource>) loader).Load(file, archiveLoader);
    }

    public void RegisterLoader<TResource>(ILoader<TResource> loader)
    {
        _loaders[typeof(TResource)] = loader;
    }
}