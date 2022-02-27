using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Core
{
    public class CoreGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly Vector2 _baseScreenSize = new Vector2(398, 224);
        private OrthographicCamera _camera;

        private SpriteFont _font;
        public CoreGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            #region Configure camera

            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();
            
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, (int) _baseScreenSize.X,
                (int) _baseScreenSize.Y);
            _camera = new OrthographicCamera(viewportAdapter);

            #endregion
            

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/TinyUnicode");
        }

        protected override void Update(GameTime gameTime)
        {
            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var backgroundColor = new Color(31, 14, 28);
            GraphicsDevice.Clear(Color.CornflowerBlue); // todo: replace by backgroundColor

            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp);
            // Draw game here
            _spriteBatch.End();

            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix(new Vector2()), samplerState: SamplerState.PointClamp);
            // Draw UI here
            _spriteBatch.FillRectangle(new Vector2(), new Size2(_baseScreenSize.X * .35f, _baseScreenSize.Y), backgroundColor);
            _spriteBatch.DrawString(_font, "test string", new Vector2(10, 10), Color.White);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}