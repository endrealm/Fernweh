using Core.Content;
using Core.Scenes.Ingame.Battle;
using Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class IngameScene: Scene
{
    private readonly GameView _gameView;
    private readonly ChatView _chatView;
    private readonly ScriptLoader _scriptLoader;
    private readonly StateRegistry _stateRegistry = new();
    private readonly BattleRegistry _battleRegistry = new();
    private readonly GameManager _gameManager;
    private readonly IFontManager _fontManager;

    /// <summary>
    /// Move to translation manager system to allow concatenating multiple files
    /// </summary>
    private DialogTranslationData _translationData;

    private Color _backgroundColor = Color.Pink;

    public IngameScene(IFontManager fontManager)
    {
        _fontManager = fontManager;
        _scriptLoader = new(_stateRegistry, _battleRegistry);
        _gameManager = new(_stateRegistry);
        _gameView = new(_stateRegistry.GlobalEventHandler, _gameManager);
        _chatView = new();
        _gameManager.StateChangedEvent += OnStateChanged;
    }

    private void OnStateChanged(StateChangedEventArgs args)
    {
        var renderer = new StateRenderer(_translationData, Language.EN_US, _fontManager, (color) => _backgroundColor = color);
        args.NewState.Render(renderer, new RenderContext(_gameManager));
        _chatView.RenderResults(renderer);
    }

    public override void Load(ContentLoader content)
    {
        _translationData = content.LoadTranslationData("Dialogs/test");
        _scriptLoader.LoadScript(content.LoadFile("States/test.lua"));
        _scriptLoader.LoadScript(content.LoadFile("Scripts/effects.lua"));
        _gameView.Load(content);
        _chatView.Load(content);
        _gameManager.LoadState("my_state"); // selects initial state
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _gameView.Update(deltaTime, new IngameUpdateContext(context));
        _chatView.Update(deltaTime, new IngameUpdateContext(context));
    }

    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(_backgroundColor);
        var backgroundColor = _backgroundColor;

        // width of text area
        int chatWidth = (int)context.BaseScreenSize.X -  (int)context.BaseScreenSize.Y;

        // rectangle culling mask in world space
        var worldCulling = new RectangleF(
            context.Camera.ScreenToWorld(new Vector2()) + new Vector2(chatWidth, 0), 
            new Size2(context.BaseScreenSize.X - chatWidth, context.BaseScreenSize.Y)
        );

        var subContext = new IngameRenderContext(context.BaseScreenSize, chatWidth, backgroundColor, worldCulling, context);
            
        var transformMatrix = context.Camera.GetViewMatrix();
        
        // Draw game world
        spriteBatch.Begin(
            transformMatrix: transformMatrix,
            samplerState: SamplerState.PointClamp
        );
        _gameView.Render(spriteBatch, subContext);
        spriteBatch.End();

        // Draw chat UI overlay
        spriteBatch.Begin(
            transformMatrix: context.Camera.GetViewMatrix(new Vector2()),
            samplerState: SamplerState.PointClamp
        );
        _chatView.Render(spriteBatch, subContext);
        spriteBatch.End();
    }
}