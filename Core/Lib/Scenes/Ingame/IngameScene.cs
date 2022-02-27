using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame;

public class IngameScene: Scene
{
    private SpriteFont _font;

    public override void Load(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/TinyUnicode");
    }

    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        var backgroundColor = new Color(31, 14, 28);
            
        // width of text area
        var chatWidth = context.BaseScreenSize.X * .35f;
            
        context.GraphicsDevice.Clear(Color.CornflowerBlue); // todo: replace by backgroundColor
            
        // rectangle culling mask in world space
        var worldCulling = new RectangleF(
            context.Camera.ScreenToWorld(new Vector2()) + new Vector2(chatWidth, 0), 
            new Size2(context.BaseScreenSize.X - chatWidth, context.BaseScreenSize.Y)
        );
            
        var transformMatrix = context.Camera.GetViewMatrix();
        spriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp);
        // Draw game here
        spriteBatch.End();

        spriteBatch.Begin(transformMatrix: context.Camera.GetViewMatrix(new Vector2()), samplerState: SamplerState.PointClamp);
        // Draw UI here
        spriteBatch.FillRectangle(new Vector2(), new Size2(chatWidth, context.BaseScreenSize.Y), backgroundColor);
        spriteBatch.DrawString(_font, "test string", new Vector2(10, 10), Color.White);
        spriteBatch.End();
    }
}