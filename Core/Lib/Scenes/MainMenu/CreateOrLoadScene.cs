using Core.Gui;
using Core.Input;
using Core.Scenes.Ingame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Core.Scenes.MainMenu;

public class CreateOrLoadScene: Scene
{
    
    private IFontManager _fontManager;
    private Action _quit;


    public CreateOrLoadScene(IFontManager fontManager, Action quit)
    {
        _fontManager = fontManager;
        _quit = quit;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        if (Button.Put("Settings").Clicked) {
            SceneManager.LoadScene(new SettingsScene(_fontManager, this));
        }
        MenuPanel.Push();
        if (Button.Put("New Game").Clicked) {
            SceneManager.LoadScene(new CreateGameScene(_fontManager, _quit));
        }
        if (Button.Put("Load Game").Clicked) {
            SceneManager.LoadScene(new LoadGameScene(_fontManager, _quit));
        }
        if (Button.Put("Quit").Clicked)
        {
            _quit.Invoke();
        }
        MenuPanel.Pop();
    }



    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.CornflowerBlue);
    }
}