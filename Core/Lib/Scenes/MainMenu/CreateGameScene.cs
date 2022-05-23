using System;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Gui;
using Core.Saving;
using Core.Scenes.Ingame;
using Core.Scenes.Modding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class CreateGameScene: Scene
{
    
    private IFontManager _fontManager;
    private Action _quit;

    private string _gameName = "";
    private int _currentModIndex = 0;
    private bool _nameUsed = false;
    
    public CreateGameScene(IFontManager fontManager, Action quit)
    {
        _fontManager = fontManager;
        _quit = quit;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        if (Button.Put("Back").Clicked) {
            SceneManager.LoadScene(new CreateOrLoadScene(_fontManager, _quit));
        }
        MenuPanel.Push();
        Label.Put("Create new game?");
        var old = _gameName;
        var box = Textbox.Put(ref _gameName, "New Game");

        if (_gameName.Length > 16 || ContainsInvalidCharacters(_gameName))
        {
            _gameName = old;
            box.Text = old;
        }
        
        // see if we need to check if file is valid
        if (_gameName != old)
        {
            CheckForNameUsage(context.SaveGameManager);
        }
        
        Horizontal.Push();
        Label.Put("Save Name:");
        Label.Put(BuildProdName(), color: Color.Wheat);
        Horizontal.Pop();
        
        Horizontal.Push();
        if (Button.Put("<").Clicked)
        {
            Cycle(-1, context.ModLoader);
        }

        var currentMod = context.ModLoader.GetGameMods().ElementAt(_currentModIndex);
        Label.Put(currentMod.Id);
        if (Button.Put(">").Clicked)
        {
            Cycle(1, context.ModLoader);
        }
        Horizontal.Pop();

        if (_nameUsed)
        {
            Label.Put("Save name already used!", color: Color.Red);
        }
        
        var validName = IsValidName();
        if (Button.Put("Start Game", color: validName ? Color.White : Color.Gray).Clicked && validName)
        {
            var gameSave = context.SaveGameManager.CreateNew(BuildProdName());
            gameSave.Data.Add("Mod", currentMod.Id);
            SceneManager.LoadScene(new IngameScene(_fontManager, context.ModLoader, currentMod.Id, gameSave));
        }
        MenuPanel.Pop();
    }

    private bool ContainsInvalidCharacters(string gameName)
    {
        return gameName.Length > 0 && !Regex.IsMatch(gameName, @"^[a-zA-Z0-9_ ]+$");
    }

    private void Cycle(int amount, ModLoader modLoader)
    {
        _currentModIndex += amount;
        if (_currentModIndex < 0)
        {
            _currentModIndex = modLoader.GetGameMods().Count();
        }

        if (_currentModIndex >= modLoader.GetGameMods().Count())
        {
            _currentModIndex = 0;
        }
    }

    private bool IsValidName()
    {
        return BuildProdName().Length > 0 && !_nameUsed;
    }

    private string BuildProdName()
    {
        return _gameName.Trim().ToLower().Replace((char)32, '_');
    }
    private void CheckForNameUsage(ISaveGameManager saveGameManager)
    {
        _nameUsed = saveGameManager.Exists(BuildProdName());
    }


    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.DarkCyan);
    }
}