using Core.Content;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Modding;
using Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class IngameScene: Scene
{
 
    private readonly ScriptLoader _scriptLoader;
    private readonly StateRegistry _stateRegistry = new();
    private readonly BattleRegistry _battleRegistry = new();
    private GameManager _gameManager;
    private ModLoader _modLoader;
    private readonly IFontManager _fontManager;

    /// <summary>
    /// Move to translation manager system to allow concatenating multiple files
    /// </summary>
    private DialogTranslationData _translationData;


    public IngameScene(IFontManager fontManager)
    {
        _fontManager = fontManager;
        _scriptLoader = new(_stateRegistry, _battleRegistry);
    }

    public override void Load(ContentLoader content)
    {
        _translationData = content.LoadTranslationData("Dialogs/test");
        _gameManager = new GameManager(_battleRegistry, _stateRegistry, _fontManager, _translationData);
        _modLoader = new ModLoader(content.GetMods());
        _modLoader.Load(_scriptLoader, "core");
        _gameManager.Load(content);
        _gameManager.StateManager.LoadState("my_state"); // selects initial state
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _gameManager.Mode.GameView.Update(deltaTime, new IngameUpdateContext(context));
        _gameManager.Mode.ChatView.Update(deltaTime, new IngameUpdateContext(context));
    }

    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(_gameManager.Mode.Background);
        var backgroundColor = _gameManager.Mode.Background;

        // width of text area
        int chatWidth = (int)context.BaseScreenSize.X -  (int)context.BaseScreenSize.Y;

        // rectangle culling mask in world space
        var worldCulling = new RectangleF(
            context.Camera.ScreenToWorld(new Vector2()) + new Vector2(chatWidth, 0), 
            new Size2(context.BaseScreenSize.X - chatWidth, context.BaseScreenSize.Y)
        );

        var subContext = new IngameRenderContext(context.BaseScreenSize, chatWidth, backgroundColor, worldCulling, context);
            
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