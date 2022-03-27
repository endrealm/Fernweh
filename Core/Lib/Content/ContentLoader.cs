using System;
using System.IO;
using Core.Utils;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary;

namespace Core.Content;

public enum ContentOrigin
{
    Core,
    Mod
}

public class ContentLoader
{

    private readonly IArchiveLoader _coreContent;
    [CanBeNull] private IArchiveLoader _modContent;
    private readonly ContentManager _contentManager;

    private readonly IFontManager _fontManager;
    
    private readonly GraphicsDeviceManager _deviceManager;

    public ContentLoader(GraphicsDeviceManager deviceManager, ContentManager contentManager, IFontManager fontManager, IArchiveLoader coreContent, [CanBeNull] IArchiveLoader modContent = null)
    {
        _coreContent = coreContent;
        _modContent = modContent;
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

    public string LoadFile(string file, ContentOrigin origin = ContentOrigin.Core)
    {
        IArchiveLoader archiveLoader;
        if (origin == ContentOrigin.Core)
        {
            archiveLoader = _coreContent;
        }
        else
        {
            archiveLoader = _modContent ?? throw new Exception("No mod loaded");
        }

        return archiveLoader.LoadFile(file);
    }
    
    public Stream LoadFileAsString(string file, ContentOrigin origin = ContentOrigin.Core)
    {
        IArchiveLoader archiveLoader;
        if (origin == ContentOrigin.Core)
        {
            archiveLoader = _coreContent;
        }
        else
        {
            archiveLoader = _modContent ?? throw new Exception("No mod loaded");
        }

        return archiveLoader.LoadFileAsStream(file);
    }

    public Texture2D LoadTexture(string file)
    {
        using (var stream = LoadFileAsString(file))
        {
            var device = _deviceManager.GraphicsDevice;
            return Texture2D.FromStream(device, stream);
        } 
    }

    public DialogTranslationData LoadTranslationData(string file)
    {
        return _contentManager.Load<DialogTranslationData>(file);
    }

}