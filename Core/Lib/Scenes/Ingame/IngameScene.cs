﻿using Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Core.Utils;

namespace Core.Scenes.Ingame;

public class IngameScene: Scene
{
    private readonly GameView _gameView = new();
    private readonly ChatView _chatView;
    private readonly StateRegistry _stateRegistry = new();
    private readonly GameManager _gameManager;

    public IngameScene()
    {
        _gameManager = new(_stateRegistry);
        _chatView = new(_gameManager);
    }

    public override void Load(ContentManager content)
    {
        _stateRegistry.LoadScript( content.Load<string>("States/test"));
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
        context.GraphicsDevice.Clear(new Color(18, 14, 18));
        var backgroundColor = new Color(18, 14, 18);

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