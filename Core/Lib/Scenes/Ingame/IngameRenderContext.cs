using Core.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Core.Scenes.Ingame;

public class IngameRenderContext : IRenderContext
{
    public IngameRenderContext(
        Vector2 baseScreenSize,
        int chatWidth,
        Color backgroundColor,
        RectangleF worldCulling,
        TopLevelRenderContext topLevelContext)
    {
        ChatWidth = chatWidth;
        BackgroundColor = backgroundColor;
        WorldTint = Color.White;
        WorldCulling = worldCulling;
        BaseScreenSize = baseScreenSize;
        TopLevelContext = topLevelContext;
    }

    public Vector2 BaseScreenSize { get; }
    public int ChatWidth { get; }
    public Color BackgroundColor { get; }
    public Color WorldTint { get; }
    public RectangleF WorldCulling { get; }
    public TopLevelRenderContext TopLevelContext { get; }
}