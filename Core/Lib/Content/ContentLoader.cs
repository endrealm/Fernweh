using System;
using System.Collections.Generic;
using System.IO;
using Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using PipelineExtensionLibrary;

namespace Core.Content;

public enum ContentOrigin
{
    Core,
    Mod
}

public class ContentLoader
{

    private readonly List<IArchiveLoader> _mods;
    private readonly ContentManager _contentManager;
    private readonly IFontManager _fontManager;
    private readonly GraphicsDeviceManager _deviceManager;

    public ContentLoader(GraphicsDeviceManager deviceManager, ContentManager contentManager, IFontManager fontManager, List<IArchiveLoader> mods)
    {
        _mods = mods;
        _deviceManager = deviceManager;
        _contentManager = contentManager;
        _fontManager = fontManager;
    }

    public void LoadFonts()
    {
        _fontManager.Load(_contentManager);
    }

    public IFontManager GetFontManager()
    {
        return _fontManager;
    }

    public string LoadFile(string file, int modId = 0)
    {
        var archiveLoader = _mods.ToArray()[modId];
        return archiveLoader.LoadFile(file);
    }
    
    public Stream LoadFileAsString(string file, int modId = 0)
    {
        var archiveLoader = _mods.ToArray()[modId];
        return archiveLoader.LoadFileAsStream(file);
    }

    public Texture2D LoadTexture(string file)
    {
        using var stream = LoadFileAsString(file);
        var device = _deviceManager.GraphicsDevice;
        return Texture2D.FromStream(device, stream);
    }

    public DialogTranslationData LoadTranslationData(string file)
    {
        return _contentManager.Load<DialogTranslationData>(file);
    }

    public List<IArchiveLoader> GetMods()
    {
        return _mods;
    }
}