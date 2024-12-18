﻿using System.Collections.Generic;
using Core.Content;
using Core.Gui;
using Core.Input;
using Core.Saving;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.MainMenu;
using Core.Scenes.Modding;
using Core.States;
using Core.Utils;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Core;

public class CoreGame : Game, ISceneManager
{
    private readonly Vector2 _baseScreenSize = new(398, 224);
    private readonly IUpdateableClickInput _clickInput;

    private readonly ContentLoader _contentLoader;
    private readonly Controls _controls = new();
    private readonly FrameCounter _frameCounter = new();
    private readonly GraphicsDeviceManager _graphics;
    private readonly ILocalizationManager _localizationManager = new BasicLocalizationManager();
    private readonly ModLoader _modLoader;
    private readonly ISaveGameManager _saveGameManager;

    private readonly string _windowName = "Fernweh";

    private Scene _activeScene;
    private OrthographicCamera _camera;
    private TopLevelRenderContext _renderContext;
    private SpriteBatch _spriteBatch;
    private IMGUI _ui;

    public CoreGame(IUpdateableClickInput clickInput, ISaveGameManager saveGameManager, List<IArchiveLoader> mods)
    {
        new GameSettings();
        _clickInput = clickInput;
        _saveGameManager = saveGameManager;
        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";

        _modLoader = new ModLoader(mods);
        _contentLoader = new ContentLoader(_graphics, Content, new SimpleFontManager(), mods)
        {
            ModLoader = _modLoader
        };

        IsMouseVisible = true;
    }

    public void LoadScene(Scene scene)
    {
        _activeScene?.Unload(); // Unload old scene if exists
        scene.InjectSceneManager(this);
        scene.Load(_contentLoader);
        _activeScene = scene;
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        var fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
        fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/TinyUnicode.ttf"));
        GuiHelper.Setup(this, fontSystem);
        _ui = new IMGUI();
        GuiHelper.CurrentIMGUI = _ui;

        GuiHelper.GuiSampler = SamplerState.PointClamp;

        // Init lua sandbox script
        LuaSandbox.Sandbox = Content.Load<string>("Scripts/sandbox");

        // todo: load default UI locale here
        // _localizationManager.Load(Content.Load<>(""));

        // Init font manager
        _contentLoader.LoadFonts();

        GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

        #region Configure camera

        UpdateVideo();
        GameSettings.Instance.OnVideoSettingsChanged += UpdateVideo;

        Window.Title = _windowName;

        var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, (int) _baseScreenSize.X,
            (int) _baseScreenSize.Y);
        _camera = new OrthographicCamera(viewportAdapter);

        #endregion


        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Scene menuScene = new MainMenuScene(_localizationManager, _contentLoader.GetFontManager(), () => { Exit(); });

        menuScene.Load(_contentLoader);
        LoadScene(menuScene);

        // Reuse instance to improve performance
        _renderContext = new TopLevelRenderContext(GraphicsDevice, _camera, _baseScreenSize);
    }

    protected override void Update(GameTime gameTime)
    {
        var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        InteractionHelper.CursorHandled = false;
        // Call UpdateSetup at the start.
        GuiHelper.UpdateSetup(gameTime);
        _ui.UpdateAll(gameTime);

        base.Update(gameTime);

        _clickInput.Update(gameTime);
        _activeScene.UiLayer.Update(deltaTime,
            new TopLevelUpdateContext(_clickInput, _camera, _modLoader, _saveGameManager));
        _activeScene.Update(deltaTime, new TopLevelUpdateContext(_clickInput, _camera, _modLoader, _saveGameManager));
        _controls.Update(deltaTime, new TopLevelUpdateContext(_clickInput, _camera, _modLoader, _saveGameManager));

        GuiHelper.UpdateCleanup();
    }

    protected override void Draw(GameTime gameTime)
    {
        // Main Rendering
        _activeScene.Render(_spriteBatch, _renderContext);
        _activeScene.UiLayer.Render(_spriteBatch, _renderContext);

        if (GameSettings.Instance.showFPS)
        {
            // Render FPS overlay
            _frameCounter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            var fps = $"FPS: {_frameCounter.AverageFramesPerSecond}";

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_contentLoader.GetFontManager().GetChatFont(), fps, new Vector2(3, -3), Color.Lime);
            _spriteBatch.End();
        }

        _ui.Draw(gameTime);

        base.Draw(gameTime);
    }

    private void UpdateVideo()
    {
        // make game borderless fullscreen
        if (GameSettings.Instance.Fullscreen)
        {
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = (int) _baseScreenSize.X * 2;
            _graphics.PreferredBackBufferHeight = (int) _baseScreenSize.Y * 2;
            _graphics.IsFullScreen = false;
        }

        _graphics.HardwareModeSwitch = false;
        _graphics.ApplyChanges();
    }
}