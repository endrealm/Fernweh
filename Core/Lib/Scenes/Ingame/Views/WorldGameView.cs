using Core.Scenes.Ingame.World;
using Core.States;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Views;

public class WorldGameView: IGameView
{
    private readonly WorldRenderer _worldRenderer;
    //private BattleRenderer battleRenderer;
    public WorldGameView(IGlobalEventHandler eventHandler, IStateManager gameManager)
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