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
        Horizontal.Push();
        Label.Put("Master Volume").PrefWidth = context.Camera.BoundingRectangle.Width/2;
        Slider.Put(ref GameSettings.Instance.Volume, 0, 1);
        Horizontal.Pop();
        Horizontal.Push();
        Label.Put("Volume").PrefWidth = context.Camera.BoundingRectangle.Width/2;
        Slider.Put(ref GameSettings.Instance.Volume, 0, 1);
        Horizontal.Pop();
        MenuPanel.Pop();
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