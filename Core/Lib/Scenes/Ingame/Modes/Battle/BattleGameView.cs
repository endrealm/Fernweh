﻿using System.Collections.Generic;
using System.Linq;
using Core.Content;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Localization;
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
    private readonly ILocalizationManager _localizationManager;

    public BattleGameView(IBattleSpriteManager battleSpriteManager, IFontManager fontManager,
        ILocalizationManager localizationManager)
    {
        _battleSpriteManager = battleSpriteManager;
        _fontManager = fontManager;
        _localizationManager = localizationManager;
    }

    public void LoadBattle(BattleManager manager)
    {
        _manager = manager;
        _statsRows = manager.Friendlies.Select(participant => new PlayerStatsRow(participant, _fontManager, _localizationManager)).ToList();
        _friendlyAvatars = manager.Friendlies.Select(participant => new BattleAvatar(_battleSpriteManager, participant)).ToList();
        _enemyAvatars = manager.Enemies.Select(participant => new BattleAvatar(_battleSpriteManager, participant)).ToList();
    }
    
    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // draw player stat bars
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

        // draw player participants 
        var halfPlayerAvatar = BattleAvatar.PlayerSize / 2;
        for (var i = 0; i < _friendlyAvatars.Count; i++)
        {
            _friendlyAvatars[i].Render(spriteBatch, new BattleAvatarRenderContext(
                i,
                new Vector2(context.BaseScreenSize.X - halfPlayerAvatar.X - 32, -halfPlayerAvatar.Y + 15),
                BattleAvatar.PlayerSize
            ));
        }

        // draw enemy participants
        var halfAvatar = BattleAvatar.DefaultSize / 2;
        for (var i = 0; i < _enemyAvatars.Count; i++)
        {
            _enemyAvatars[i].Render(spriteBatch, new BattleAvatarRenderContext(
                i,
                new Vector2(context.ChatWidth + halfAvatar.X + 12, -halfAvatar.Y + 15),
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