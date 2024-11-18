using Microsoft.Xna.Framework.Graphics;

namespace Core.Utils;

public interface IRenderer<T> where T : IRenderContext
{
    void Render(SpriteBatch spriteBatch, T context);
}