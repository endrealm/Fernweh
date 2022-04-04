using System.Collections.Generic;
using System.Linq;
using Core.Content;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleGameView: IGameView
{
    private BattleManager _manager;
    private List<PlayerStatsRow> _statsRows = new();
    private List<BattleAvatar> _friendlyAvatars = new();
    private List<BattleAvatar> _enemyAvatars = new();
    private readonly IBattleSpriteManager _battleSpriteManager;
    private IFontManager _fontManager;
    private readonly DialogTranslationData _translationData;

    public BattleGameView(IBattleSpriteManager battleSpriteManager, IFontManager fontManager,
        DialogTranslationData translationData)
    {
        _battleSpriteManager = battleSpriteManager;
        _fontManager = fontManager;
        _translationData = translationData;
    }

    public void LoadBattle(BattleManager manager)
    {
        _manager = manager;
        _statsRows = manager.Friendlies.Select(participant => new PlayerStatsRow(participant, _fontManager, _translationData)).ToList();
        _friendlyAvatars = manager.Friendlies.Select(participant => new BattleAvatar(_battleSpriteManager, participant)).ToList();
        _enemyAvatars = manager.Enemies.Select(participant => new BattleAvatar(_battleSpriteManager, participant)).ToList();
    }
    
    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        for (var i = 0; i < _statsRows.Count; i++)
        {
            _statsRows[i].Render(spriteBatch, new PlayerStatsRowRenderContext(
                i,
                _statsRows.Count,
                context.BaseScreenSize,
                context.ChatWidth,
                (int)(context.BaseScreenSize.X-context.ChatWidth)
            ));
        }

        var halfPlayerAvatar = BattleAvatar.PlayerSize / 2;
        for (var i = 0; i < _friendlyAvatars.Count; i++)
        {
            _friendlyAvatars[i].Render(spriteBatch, new BattleAvatarRenderContext(
                i,
                new Vector2(context.BaseScreenSize.X - halfPlayerAvatar.X, -halfPlayerAvatar.Y),
                BattleAvatar.PlayerSize
            ));
        }
        var halfAvatar = BattleAvatar.DefaultSize / 2;
        for (var i = 0; i < _enemyAvatars.Count; i++)
        {
            _enemyAvatars[i].Render(spriteBatch, new BattleAvatarRenderContext(
                i,
                new Vector2(context.ChatWidth + halfAvatar.X, -halfAvatar.Y),
                BattleAvatar.DefaultSize,
                3,
                false
            ));
        }
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
    }

    public void Load(ContentLoader content)
    {
    }

    public bool WorldSpacedCoordinates => false;
}