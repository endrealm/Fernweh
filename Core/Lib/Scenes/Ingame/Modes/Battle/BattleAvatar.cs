using System;
using Core.Scenes.Ingame.Battle;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleAvatar: IRenderer<BattleAvatarRenderContext>
{
    public static readonly Vector2 DefaultSize = new(40,40);
    public static readonly Vector2 PlayerSize = new(19,29);
    private readonly IBattleSpriteManager _spriteManager;
    private readonly IBattleParticipant _participant;
    private Texture2D _sprite;

    public BattleAvatar(IBattleSpriteManager spriteManager, IBattleParticipant participant)
    {
        _spriteManager = spriteManager;
        _participant = participant;
        AssignTexture();
    }

    private void AssignTexture()
    {
        _sprite = _spriteManager.GetTexture(_participant.GroupId);
    }

    public void Render(SpriteBatch spriteBatch, BattleAvatarRenderContext context)
    {
        if(_participant.State != ParticipantState.Alive) return;
        Vector2 calcPos;
        if (context.RowsHorizontal)
        {
            calcPos = context.Position + context.Bounds * new Vector2(context.ItemNr % context.MaxRowItems,
                (int)Math.Floor(context.ItemNr / (float)context.MaxRowItems));
        }
        else
        {
            calcPos = context.Position + context.Bounds * new Vector2((int)Math.Floor(context.ItemNr / (float)context.MaxRowItems), context.ItemNr % context.MaxRowItems);
        }
        spriteBatch.Draw(_sprite, calcPos - new Vector2(_sprite.Width/2f, -_sprite.Height/2f), Color.White);
    }
}

public class BattleAvatarRenderContext : IRenderContext
{
    
    public Vector2 Position { get; }
    public Vector2 Bounds { get; }
    public int MaxRowItems { get; }
    public bool RowsHorizontal { get; }
    public int ItemNr { get; }

    public BattleAvatarRenderContext(int itemNr, Vector2 position, Vector2 bounds, int maxRowItems = 1, bool rowsHorizontal = true)
    {
        ItemNr = itemNr;
        Position = position;
        Bounds = bounds;
        MaxRowItems = maxRowItems;
        RowsHorizontal = rowsHorizontal;
    }

}