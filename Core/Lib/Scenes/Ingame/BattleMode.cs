using System.Collections.Generic;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame;

public class BattleMode : IMode
{

    private readonly BattleChatView _chatView;
    public Color Background { get; } = new(18, 14, 18);
    public IChatView ChatView => _chatView;
    public IGameView GameView { get; }

    private readonly BattleRegistry _battleRegistry;

    public BattleMode(BattleRegistry battleRegistry)
    {
        _battleRegistry = battleRegistry;

        _chatView = new BattleChatView();
        GameView = new BattleGameView();
    }

    public void Load(ModeParameters parameters)
    {
        var battleManager = new BattleManager(_battleRegistry, parameters.GetValue<BattleConfig>("config"), _chatView);
    }
}