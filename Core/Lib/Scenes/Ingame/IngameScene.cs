using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame;

public class IngameScene: Scene
{
    private readonly GameView _gameView = new();
    private readonly ChatView _chatView = new();

    public override void Load(ContentManager content)
    {
        _gameView.Load(content);
        _chatView.Load(content);
    }

    public override void Update(float deltaTime, TopLeveUpdateContext context)
    {
        _gameView.Update(deltaTime, new IngameUpdateContext());
        _chatView.Update(deltaTime, new IngameUpdateContext());
    }

    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(new Color(48, 37, 45));
        var backgroundColor = new Color(18, 14, 18);
            
        // width of text area. if accepting wider aspect ratios, this should increase
        var chatWidth = 174;
            
            
        // rectangle culling mask in world space
        var worldCulling = new RectangleF(
            context.Camera.ScreenToWorld(new Vector2()) + new Vector2(chatWidth, 0), 
            new Size2(context.BaseScreenSize.X - chatWidth, context.BaseScreenSize.Y)
        );

        var subContext = new IngameRenderContext(context.BaseScreenSize, chatWidth, backgroundColor, worldCulling);
            
        var transformMatrix = context.Camera.GetViewMatrix();
        
        // Draw game world
        spriteBatch.Begin(
            transformMatrix: transformMatrix,
            samplerState: SamplerState.PointClamp,
            blendState: BlendState.NonPremultiplied
        );
        _gameView.Render(spriteBatch, subContext);
        spriteBatch.End();

        // Draw chat UI overlay
        spriteBatch.Begin(
            transformMatrix: context.Camera.GetViewMatrix(new Vector2()),
            samplerState: SamplerState.PointClamp,
            blendState: BlendState.NonPremultiplied
        );
        _chatView.Render(spriteBatch, subContext);
        spriteBatch.End();
    }
}