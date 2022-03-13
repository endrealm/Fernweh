using Core.Input;
using Core.Scenes.MainMenu;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

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
        private FrameCounter _frameCounter = new();

        private Scene _activeScene;
        private TopLevelRenderContext _renderContext;
        private SpriteFont _font;

        public CoreGame(IUpdateableClickInput clickInput)
        {
            _clickInput = clickInput;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _font = Content.Load<SpriteFont>("Fonts/TinyUnicode");

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

            LoadScene(new MainMenuScene());

            // Reuse instance to improve performance
            _renderContext = new TopLevelRenderContext(GraphicsDevice, _camera, _baseScreenSize);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _clickInput.Update(gameTime);
            _activeScene.Update((float) gameTime.ElapsedGameTime.TotalSeconds, new TopLevelUpdateContext(_clickInput, _camera));
        }

        protected override void Draw(GameTime gameTime)
        {
            _frameCounter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            
            // Main Rendering
            _activeScene.Render(_spriteBatch, _renderContext);
            
            // Render FPS overlay
            var fps = $"FPS: {_frameCounter.AverageFramesPerSecond}";
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, fps, Vector2.Zero, Color.Gold);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public void LoadScene(Scene scene)
        {
            _activeScene?.Unload(); // Unload old scene if exists
            scene.InjectSceneManager(this);
            scene.Load(Content);
            _activeScene = scene;
        }
    }
}