using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Core.Scenes.Ingame.World;
using System;
using Core.Input;
using Core.States;

namespace Core.Scenes.Ingame;

public class GameView: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    private readonly WorldRenderer _worldRenderer;
    //private BattleRenderer battleRenderer;
    public GameView(IGlobalEventHandler eventHandler, GameManager gameManager)
    {
        _worldRenderer = new WorldRenderer(eventHandler, gameManager);
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        _worldRenderer.Render(spriteBatch, context);
    }

    public void Load(ContentManager content)
    {
        _worldRenderer.Load(content);
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        _worldRenderer.Update(deltaTime, context);
    }
}