using System;
using System.Collections.Generic;
using Core.Gui;
using Core.Input;
using Core.Saving;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class LoadGameScene: Scene
{
    private Action _quit;

    private List<IGameSave> _gameSaves;
    public LoadGameScene(ILocalizationManager rootLocalizationManager, IFontManager fontManager, Action quit): base(fontManager, rootLocalizationManager)
    {
        _quit = quit;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _gameSaves ??= context.SaveGameManager.ListAll();
        if (Button.Put("Back").Clicked) {
            SceneManager.LoadScene(new CreateOrLoadScene(RootLocalizationManager, FontManager, _quit));
        }
        MenuPanel.Push();
        
        foreach (var gameSave in _gameSaves)
        {
            if (Button.Put(gameSave.Name).Clicked) {
                gameSave.Load();
                SceneManager.LoadScene(new IngameScene(RootLocalizationManager, FontManager, context.ModLoader, (string) gameSave.Data["Mod"], gameSave));
            }
        }
        MenuPanel.Pop();
    }



    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.Firebrick);
    }
}