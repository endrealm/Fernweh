using System;
using Core.Gui;
using Core.Scenes.Ingame.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class CreateOrLoadScene : Scene
{
    private readonly Action _quit;


    public CreateOrLoadScene(ILocalizationManager rootLocalizationManager, IFontManager fontManager, Action quit) :
        base(fontManager, rootLocalizationManager)
    {
        _quit = quit;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        if (Button.Put("Settings").Clicked)
            SceneManager.LoadScene(new SettingsScene(RootLocalizationManager, FontManager, this));
        MenuPanel.Push();
        if (Button.Put("New Game").Clicked)
            SceneManager.LoadScene(new CreateGameScene(RootLocalizationManager, FontManager, _quit));
        if (Button.Put("Load Game").Clicked)
            SceneManager.LoadScene(new LoadGameScene(RootLocalizationManager, FontManager, _quit));
        if (Button.Put("Quit").Clicked) _quit.Invoke();
        MenuPanel.Pop();
    }


    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(new Color(18, 14, 18));
    }
}