using Core.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Core.Scenes.Ingame;

public class IngameRenderContext: IRenderContext
{
    public Vector2 BaseScreenSize { get; }
    public float ChatWidth { get; }
    public Color BackgroundColor { get; }
    public RectangleF WorldCulling { get; }

    public IngameRenderContext(
        Vector2 baseScreenSize,
        float chatWidth,
        Color backgroundColor, 
        RectangleF worldCulling
    )
    {
        ChatWidth = chatWidth;
        BackgroundColor = backgroundColor;
        WorldCulling = worldCulling;
        BaseScreenSize = baseScreenSize;
    }
}