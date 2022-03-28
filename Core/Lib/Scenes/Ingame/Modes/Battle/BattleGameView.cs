using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Battle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleGameView: IGameView
{
    private BattleManager _manager;
    private List<PlayerStatsRow> _statsRows = new();
    private IFontManager _fontManager;
    private readonly DialogTranslationData _translationData;

    public BattleGameView(IFontManager fontManager, DialogTranslationData translationData)
    {
        _fontManager = fontManager;
        _translationData = translationData;
    }

    public void LoadBattle(BattleManager manager)
    {
        _manager = manager;
        _statsRows = manager.Friendlies.Select(participant => new PlayerStatsRow(participant, _fontManager, _translationData)).ToList();
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
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
    }

    public void Load(ContentManager content)
    {
    }

    public bool WorldSpacedCoordinates => false;
}