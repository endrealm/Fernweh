using Core.Input;
using Core.Scenes.Ingame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class MainMenuScene: Scene
{
    
    private IFontManager _fontManager;

    private float _alpha = 255;
    private bool _ascending;

    public MainMenuScene(IFontManager fontManager)
    {
        _fontManager = fontManager;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        UpdateAlpha(deltaTime);
        if (Controls.AnyInput())
        {
            SceneManager.LoadScene(new IngameScene(_fontManager));
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
        
        spriteBatch.Begin(
            transformMatrix: context.Camera.GetViewMatrix(new Vector2()), // preserve screen spaced values
            samplerState: SamplerState.PointClamp, // renders pixel perfect -> no blurry edges
            sortMode: SpriteSortMode.Immediate // no clue, but doesnt do any harm?
        );
        
        const string message = "Press any button to continue";
        var measurement = _fontManager.GetChatFont().MeasureString(message);
        spriteBatch.DrawString(_fontManager.GetChatFont(), message, context.BaseScreenSize/2 - measurement/2, new Color(255,255,255) * (_alpha/255f));
        
        spriteBatch.End();
    }
}