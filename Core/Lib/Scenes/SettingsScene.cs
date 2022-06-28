using Core.Gui;
using Core.Scenes.MainMenu;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Core.Scenes.Ingame.Localization;

namespace Core.Scenes;

public class SettingsScene: Scene
{
    
    private readonly Scene _previousScene;

    public SettingsScene(ILocalizationManager rootLocalizationManager, IFontManager fontManager, Scene previousScene): base(fontManager, rootLocalizationManager)
    {
        _previousScene = previousScene;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        Vertical.Push();
        if (Button.Put("Back").Clicked) {
            SceneManager.LoadScene(_previousScene);
        }
        MenuPanel.Push();
        Label.Put("Music Volume: " + (int)(GameSettings.Instance.Music*100) + "%").PrefWidth = context.Camera.BoundingRectangle.Width/2;
        Slider.Put(ref GameSettings.Instance.Music, 0, 1);
        Label.Put("SFX Volume: " + (int)(GameSettings.Instance.Sfx * 100) + "%").PrefWidth = context.Camera.BoundingRectangle.Width/2;
        Slider.Put(ref GameSettings.Instance.Sfx, 0, 1);
        Label.Put("Fullscreen").PrefWidth = context.Camera.BoundingRectangle.Width / 2;
        Checkbox.Put(ref GameSettings.Instance.Fullscreen);
        Label.Put("Typing Speed: " + GameSettings.Instance.TypingSpeed).PrefWidth = context.Camera.BoundingRectangle.Width / 2;
        Slider.Put(ref GameSettings.Instance.TypingSpeed, 0, 0.02f);
        Label.Put("Show FPS").PrefWidth = context.Camera.BoundingRectangle.Width / 2;
        Checkbox.Put(ref GameSettings.Instance.showFPS);
        Horizontal.Pop();
        MenuPanel.Pop();

        // round values + update graphics for fullscreen
        GameSettings.Instance.Music = (float)Math.Round(GameSettings.Instance.Music, 2);
        GameSettings.Instance.Sfx = (float)Math.Round(GameSettings.Instance.Sfx, 2);
        GameSettings.Instance.TypingSpeed = (float)Math.Round(GameSettings.Instance.TypingSpeed, 3);
        GameSettings.Instance.UpdateVideoSettings();
    }



    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.Coral);
    }
}

public struct SettingsConfig
{
    public SettingsConfig()
    {
    }

    public bool BackToMenu { get; } = true;
}