using Core.Gui;
using Core.Input;
using Core.Scenes.Ingame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class CreateOrLoadScene: Scene
{
    
    private IFontManager _fontManager;


    public CreateOrLoadScene(IFontManager fontManager)
    {
        _fontManager = fontManager;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        MenuPanel.Push();
        if (Button.Put("New Game").Clicked) {
            SceneManager.LoadScene(new CreateGameScene(_fontManager));
        }
        if (Button.Put("Load Game").Clicked) {
            SceneManager.LoadScene(new LoadGameScene(_fontManager));
        }
        MenuPanel.Pop();
    }



    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.CornflowerBlue);
    }
}