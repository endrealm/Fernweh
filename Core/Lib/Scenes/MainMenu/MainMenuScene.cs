using Core.Input;
using Core.Scenes.Ingame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.MainMenu;

public class MainMenuScene: Scene
{
    
    private SpriteFont _font;

    public override void Load(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/TinyUnicode");
    }

    private float _alpha = 255;
    private bool _ascending;

    public override void Update(float deltaTime, TopLeveUpdateContext context)
    {
        UpdateAlpha(deltaTime);
        if (KeyboardSnapshot.GetState().GetPressedKeys().Length > 0)
        {
            SceneManager.LoadScene(new IngameScene());
        }
    }

    private void UpdateAlpha(float deltaTime)
    {
        const int speed = 200;
        _alpha += speed * deltaTime * (_ascending ? 1 : -1);
        
        if (_ascending)
        {
            if (!(_alpha >= 255)) return;
            _alpha = 255;
            _ascending = false;
            return;
        }
        
        if (!(_alpha <= 0)) return;
        _alpha = 0;
        _ascending = true;
    }

    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.CornflowerBlue);
        
        // Matrix is used to preserve screen spaced values
        // Point clamp renders pixel perfect -> no blurry edges
        spriteBatch.Begin(
            transformMatrix: context.Camera.GetViewMatrix(new Vector2()), 
            samplerState: SamplerState.PointClamp, 
            blendState: BlendState.NonPremultiplied,
            sortMode: SpriteSortMode.Immediate
            );
        
        const string message = "Press any button to continue";
        var measurement = _font.MeasureString(message);
        spriteBatch.DrawString(_font, message, context.BaseScreenSize/2 - measurement/2, new Color(255,255,255, (int)_alpha));
        
        spriteBatch.End();
    }
}