using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Core.Scenes.Ingame.World;
using System;
using Core.Input;

namespace Core.Scenes.Ingame;

public class GameView: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    private WorldRenderer worldRenderer = new WorldRenderer();
    //private BattleRenderer battleRenderer;

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        worldRenderer.Render(spriteBatch, context);
    }

    public void Load(ContentManager content)
    {
        worldRenderer.Load(content);
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        worldRenderer.Update(deltaTime, context);
    }
}