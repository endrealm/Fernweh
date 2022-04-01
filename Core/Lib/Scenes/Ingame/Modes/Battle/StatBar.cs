using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame.Modes.Battle;

public class StatBar: IRenderer<BarRenderContext>
{

    private Color _primary;
    private Color _secondary;

    public StatBar(Color primary, Color secondary)
    {
        _primary = primary;
        _secondary = secondary;
    }

    public float Percentage { set; private get; }
    public void Render(SpriteBatch spriteBatch, BarRenderContext context)
    {
        var fullRange = (int)(context.Width * Percentage);
        for (var y = 0; y < 2; y++)
        {
            var basePos = context.Position + new Vector2(-y, y);
            var wasFilled = false;
            for (var x = 0; x < context.Width; x++)
            {
                var filled = x < fullRange;
                if (filled)
                {
                    wasFilled = true;
                }
                else if(wasFilled)
                {
                    wasFilled = false;
                    continue;
                }
                var localPos = basePos + new Vector2(x, 0);
                spriteBatch.DrawPoint(localPos, filled ? _primary : _secondary);
            }
        }
        
    }
}

public class BarRenderContext : IRenderContext
{
    public BarRenderContext(Vector2 position, int width)
    {
        Position = position;
        Width = width;
    }

    public Vector2 Position { get; }
    public int Width { get; }
}