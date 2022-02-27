using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame;

public class ChatView: IRenderer<IngameRenderContext>, ILoadable
{
    private SpriteFont _font;

    public void Load(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/TinyUnicode");
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // Draw UI here
        spriteBatch.FillRectangle(new Vector2(), new Size2(context.ChatWidth, context.BaseScreenSize.Y), context.BackgroundColor);
        spriteBatch.DrawString(_font, "test string", new Vector2(10, 10), Color.White);
    }
}