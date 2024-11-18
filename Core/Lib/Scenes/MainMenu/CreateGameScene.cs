using System;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Gui;
using Core.Saving;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Modding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class CreateGameScene : Scene
{
    private readonly Action _quit;
    private int _currentModIndex;

    private string _gameName = "";
    private bool _nameUsed;

    public CreateGameScene(ILocalizationManager rootLocalizationManager, IFontManager fontManager, Action quit) : base(
        fontManager, rootLocalizationManager)
    {
        _quit = quit;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        if (Button.Put("Back").Clicked)
            SceneManager.LoadScene(new CreateOrLoadScene(RootLocalizationManager, FontManager, _quit));
        MenuPanel.Push();
        Label.Put("Create new game?");
        var old = _gameName;
        var box = Textbox.Put(ref _gameName, "game_name");

        if (_gameName.Length > 16 || ContainsInvalidCharacters(_gameName))
        {
            _gameName = old;
            box.Text = old;
        }

        // see if we need to check if file is valid
        if (_gameName != old)
        {
            box.Text = BuildProdName();
            CheckForNameUsage(context.SaveGameManager);
        }

        Horizontal.Push();
        if (Button.Put("<").Clicked) Cycle(-1, context.ModLoader);

        var currentMod = context.ModLoader.GetGameMods().ElementAt(_currentModIndex);
        Label.Put(currentMod.Id);
        if (Button.Put(">").Clicked) Cycle(1, context.ModLoader);
        Horizontal.Pop();

        if (_nameUsed) Label.Put("Save name already used!", color: Color.Red);

        var validName = IsValidName();
        if (Button.Put("Start Game", color: validName ? Color.White : Color.Gray).Clicked && validName)
        {
            var gameSave = context.SaveGameManager.CreateNew(BuildProdName());
            gameSave.Data.Add("Mod", currentMod.Id);
            SceneManager.LoadScene(new IngameScene(RootLocalizationManager, FontManager, context.ModLoader,
                currentMod.Id, gameSave));
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
        if (_currentModIndex < 0) _currentModIndex = modLoader.GetGameMods().Count();

        if (_currentModIndex >= modLoader.GetGameMods().Count()) _currentModIndex = 0;
    }

    private bool IsValidName()
    {
        return _gameName.Length > 0 && !_nameUsed;
    }

    private string BuildProdName()
    {
        return _gameName.ToLower().Replace((char) 32, '_');
    }

    private void CheckForNameUsage(ISaveGameManager saveGameManager)
    {
        _nameUsed = saveGameManager.Exists(_gameName);
    }


    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.DarkCyan);
    }
}