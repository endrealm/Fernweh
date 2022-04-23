using System.Collections.Generic;
using Core.Content;
using Core.Gui;
using Core.Input;
using Core.Scenes.MainMenu;
using Core.Scenes.Modding;
using Core.States;
using Core.Utils;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using FontStashSharp;

namespace Core
{
    public class CoreGame : Game, ISceneManager
    {
        private readonly IUpdateableClickInput _clickInput;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;
        private readonly Vector2 _baseScreenSize = new(398, 224);
        private bool _isFullscreen = false;
        private Controls _controls = new Controls();
        private FrameCounter _frameCounter = new();
        private readonly ModLoader _modLoader;

        private readonly ContentLoader _contentLoader;
        
        private Scene _activeScene;
        private TopLevelRenderContext _renderContext;
        private IMGUI _ui;

        public CoreGame(IUpdateableClickInput clickInput, List<IArchiveLoader> mods)
        {
            _clickInput = clickInput;
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            
            _modLoader = new ModLoader(mods);
            _contentLoader = new ContentLoader(_graphics, Content, new SimpleFontManager(),  mods )
            {
                ModLoader = _modLoader
            };

            IsMouseVisible = true;
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

            // Init font manager
            _contentLoader.LoadFonts();
            
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            #region Configure camera

            // make game borderless fullscreen
            if(_isFullscreen)
            {
                _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                _graphics.IsFullScreen = true;
                _graphics.HardwareModeSwitch = false;
                _graphics.ApplyChanges();
            }
            
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, (int) _baseScreenSize.X,
                (int) _baseScreenSize.Y);
            _camera = new OrthographicCamera(viewportAdapter);

            #endregion
            

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadScene(new MainMenuScene(_contentLoader.GetFontManager()));

            // Reuse instance to improve performance
            _renderContext = new TopLevelRenderContext(GraphicsDevice, _camera, _baseScreenSize);
        }

        protected override void Update(GameTime gameTime)
        {
            // Call UpdateSetup at the start.
            GuiHelper.UpdateSetup(gameTime);
            _ui.UpdateAll(gameTime);
            
            base.Update(gameTime);

            _clickInput.Update(gameTime);
            _activeScene.Update((float) gameTime.ElapsedGameTime.TotalSeconds, new TopLevelUpdateContext(_clickInput, _camera, _modLoader));
            _controls.Update((float)gameTime.ElapsedGameTime.TotalSeconds, new TopLevelUpdateContext(_clickInput, _camera, _modLoader));
            
            GuiHelper.UpdateCleanup();
        }

        protected override void Draw(GameTime gameTime)
        {
            _frameCounter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            
            // Main Rendering
            _activeScene.Render(_spriteBatch, _renderContext);
            
            // Render FPS overlay
            var fps = $"FPS: {_frameCounter.AverageFramesPerSecond}";
            
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_contentLoader.GetFontManager().GetChatFont(), fps, Vector2.Zero, Color.Gold);
            _spriteBatch.End();
            
            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void LoadScene(Scene scene)
        {
            _activeScene?.Unload(); // Unload old scene if exists
            scene.InjectSceneManager(this);
            scene.Load(_contentLoader);
            _activeScene = scene;
        }
    }
}