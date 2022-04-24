using System.Collections.Generic;
using Core.Gui;
using Core.Input;
using Core.Saving;
using Core.Scenes.Ingame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class LoadGameScene: Scene
{
    
    
    private IFontManager _fontManager;

    private List<IGameSave> _gameSaves;
    public LoadGameScene(IFontManager fontManager)
    {
        _fontManager = fontManager;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _gameSaves ??= context.SaveGameManager.ListAll();
        if (Button.Put("Back").Clicked) {
            SceneManager.LoadScene(new CreateOrLoadScene(_fontManager));
        }
        MenuPanel.Push();
        
        foreach (var gameSave in _gameSaves)
        {
            if (Button.Put(gameSave.Name).Clicked) {
                gameSave.Load();
                SceneManager.LoadScene(new IngameScene(_fontManager, context.ModLoader, (string) gameSave.Data["Mod"], gameSave));
            }
        }
        MenuPanel.Pop();
    }



    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.Firebrick);
    }
}