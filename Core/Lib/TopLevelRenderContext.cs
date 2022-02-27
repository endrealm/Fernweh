using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core;

public class TopLevelRenderContext: IRenderContext
{
    public GraphicsDevice GraphicsDevice { get; }
    public OrthographicCamera Camera { get; }
    public Vector2 BaseScreenSize { get; }

    public TopLevelRenderContext(
        GraphicsDevice graphicsDevice,
        OrthographicCamera camera,
        Vector2 baseScreenSize
    )
    {
        GraphicsDevice = graphicsDevice;
        Camera = camera;
        BaseScreenSize = baseScreenSize;
    }
}