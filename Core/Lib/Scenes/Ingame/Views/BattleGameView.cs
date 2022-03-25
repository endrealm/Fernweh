using Core.Scenes.Ingame.Battle;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Views;

public class BattleGameView: IGameView
{
    private BattleManager _manager;
    public void LoadBattle(BattleManager manager)
    {
        _manager = manager;
    }
    
    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
    }

    public void Load(ContentManager content)
    {
    }
}