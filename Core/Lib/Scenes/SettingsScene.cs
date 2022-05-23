using Core.Gui;
using Core.Scenes.MainMenu;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes;

public class SettingsScene: Scene
{
    
    private IFontManager _fontManager;
    private readonly Scene _previousScene;

    private bool _fullscreen = GameSettings.Instance.Fullscreen;

    public SettingsScene(IFontManager fontManager, Scene previousScene)
    {
        _fontManager = fontManager;
        _previousScene = previousScene;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        Vertical.Push();
        if (Button.Put("Back").Clicked) {
            SceneManager.LoadScene(_previousScene);
        }
        MenuPanel.Push();
        Label.Put("Music Volume").PrefWidth = context.Camera.BoundingRectangle.Width/2;
        Slider.Put(ref GameSettings.Instance.Music, 0, 1);
        Label.Put("SFX Volume").PrefWidth = context.Camera.BoundingRectangle.Width/2;
        Slider.Put(ref GameSettings.Instance.Sfx, 0, 1);
        Label.Put("Fullscreen").PrefWidth = context.Camera.BoundingRectangle.Width / 2;
        Checkbox.Put(ref _fullscreen);
        Horizontal.Pop();
        MenuPanel.Pop();

        GameSettings.Instance.Fullscreen = _fullscreen;
    }



    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.CornflowerBlue);
    }
}

public struct SettingsConfig
{
    public SettingsConfig()
    {
    }

    public bool BackToMenu { get; } = true;
}