using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Content;

public class TextureLoader: ILoader<Texture2D>
{
    
    private readonly GraphicsDeviceManager _deviceManager;

    public TextureLoader(GraphicsDeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }
    
    public Texture2D Load(string file, IArchiveLoader archiveLoader)
    {
        using var stream = archiveLoader.LoadFileAsStream(file);
        var device = _deviceManager.GraphicsDevice;
        return Texture2D.FromStream(device, stream);
    }
}