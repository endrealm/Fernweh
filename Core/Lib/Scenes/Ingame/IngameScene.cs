﻿using Core.Content;
using Core.Saving;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Modding;
using Core.Scripting;
using Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame;

public class IngameScene : Scene, ISaveSystem
{
    private readonly BattleRegistry _battleRegistry = new();
    private readonly string _currentModId;
    private readonly IGameSave _gameSave;

    private readonly ILocalizationManager _localizationManager;
    private readonly ModLoader _modLoader;

    private readonly ScriptLoader _scriptLoader;
    private readonly StateRegistry _stateRegistry = new();
    private bool _allowSave;
    private GameManager _gameManager;


    public IngameScene(ILocalizationManager rootLocalizationManager, IFontManager fontManager, ModLoader modLoader,
        string currentModId, IGameSave gameSave) : base(fontManager, rootLocalizationManager)
    {
        _modLoader = modLoader;
        _currentModId = currentModId;
        _gameSave = gameSave;
        _localizationManager = new BasicLocalizationManager();
        _scriptLoader = new ScriptLoader(_stateRegistry, _battleRegistry, gameSave, _localizationManager);
    }

    public void SaveAll()
    {
        if (!_allowSave) return;
        _gameManager?.Save();
        _scriptLoader?.Save();
        _gameSave.Save();
    }

    public override void Load(ContentLoader content)
    {
        _modLoader.UnloadAllMods();

        // Loads all mods
        _modLoader.Load(_currentModId);

        _localizationManager.LoadLangs(_modLoader.ActiveModOrder, content);

        // load all assets, along with game manager
        var reg = new ContentRegistry();
        reg.LoadAllContent(content);

        _gameManager = new GameManager(reg, _battleRegistry, _stateRegistry, FontManager, _localizationManager,
            _gameSave, this);

        // Loads scripts of all mods
        _gameManager.Load(content);
        _modLoader.RunActiveModScripts(_scriptLoader);
        _scriptLoader.Load();
        _allowSave = true;
        _gameManager.LoadGameState();
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        if (_gameManager == null) return; //dont update if gamemanager hasnt loaded yet
        _gameManager.Mode.GameView.Update(deltaTime, new IngameUpdateContext(context));
        _gameManager.Mode.ChatView.Update(deltaTime, new IngameUpdateContext(context));
    }

    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        if (_gameManager == null) return; //dont render if gamemanager hasnt loaded yet

        context.GraphicsDevice.Clear(_gameManager.Mode.Background);
        var backgroundColor = _gameManager.Mode.Background;

        // width of text area
        var chatWidth = (int) context.BaseScreenSize.X - (int) context.BaseScreenSize.Y;

        // rectangle culling mask in world space
        var worldCulling = new RectangleF(
            context.Camera.ScreenToWorld(new Vector2()) + new Vector2(chatWidth, 0),
            new Size2(context.BaseScreenSize.X - chatWidth, context.BaseScreenSize.Y)
        );

        var subContext =
            new IngameRenderContext(context.BaseScreenSize, chatWidth, backgroundColor, worldCulling, context);

        var transformMatrix = _gameManager.Mode.GameView.WorldSpacedCoordinates
            ? context.Camera.GetViewMatrix()
            : context.Camera.GetViewMatrix(new Vector2());

        // Draw game world
        spriteBatch.Begin(
            transformMatrix: transformMatrix,
            samplerState: SamplerState.PointClamp
        );
        _gameManager.Mode.GameView.Render(spriteBatch, subContext);
        spriteBatch.End();

        // Draw chat UI overlay
        spriteBatch.Begin(
            transformMatrix: context.Camera.GetViewMatrix(new Vector2()),
            samplerState: SamplerState.PointClamp
        );
        _gameManager.Mode.ChatView.Render(spriteBatch, subContext);
        spriteBatch.End();
    }
}