using Core.Content;
using Core.Input;
using Core.Scenes.Ingame;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Core.Scenes.MainMenu;

public class MainMenuScene: Scene
{
    
    private IFontManager _fontManager;

    private float _alpha = 255;
    private bool _ascending;

    private Texture2D _splashLogo;

    public MainMenuScene(IFontManager fontManager)
    {
        _fontManager = fontManager;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        UpdateAlpha(deltaTime);
        if (Controls.AnyInput())
        {
            // SceneManager.LoadScene(new IngameScene(_fontManager));
            SceneManager.LoadScene(new CreateOrLoadScene(_fontManager));
        }
    }

    public override void Load(ContentLoader content)
    {
        _splashLogo = content.Load<Texture2D>("Sprites/splash_logo.png");
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
        context.GraphicsDevice.Clear(new Color(18, 14, 18));
        
        spriteBatch.Begin(
            transformMatrix: context.Camera.GetViewMatrix(new Vector2()), // preserve screen spaced values
            samplerState: SamplerState.PointClamp, // renders pixel perfect -> no blurry edges
            sortMode: SpriteSortMode.Immediate // no clue, but doesnt do any harm?
        );
        
        RenderText(spriteBatch, context, "Press any button to continue", 190, new Color(255, 255, 255) * (_alpha / 255f));

        RenderText(spriteBatch, context, "Copyright 1993 EndRealm Network", 130, Color.White);
        RenderText(spriteBatch, context, "Fernweh and its logo are", 140, Color.White);
        RenderText(spriteBatch, context, "copyright owned by EndRealm", 150, Color.White);
        RenderText(spriteBatch, context, "All rights reserved", 160, Color.White);

        spriteBatch.Draw(_splashLogo, new Rectangle(new Point(), new Point(398, 112)), Color.White);

        spriteBatch.End();
    }

    private void RenderText(SpriteBatch spriteBatch, TopLevelRenderContext context, string text, float y, Color color)
    {
        var measurement = _fontManager.GetChatFont().MeasureString(text);
        spriteBatch.DrawString(_fontManager.GetChatFont(), text, new Vector2(context.BaseScreenSize.X / 2 - measurement.X / 2, y), color);
    }
}