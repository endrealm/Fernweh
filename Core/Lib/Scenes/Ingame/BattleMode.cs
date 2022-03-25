using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class BattleMode : IMode
{

    private readonly BattleChatView _chatView;
    public Color Background { get; } = new(18, 14, 18);
    public IChatView ChatView => _chatView;
    public IGameView GameView { get; }

    private readonly BattleRegistry _battleRegistry;

    public BattleMode(BattleRegistry battleRegistry, DialogTranslationData translationData, IFontManager fontManager)
    {
        _battleRegistry = battleRegistry;

        _chatView = new BattleChatView(translationData, fontManager);
        GameView = new BattleGameView();
    }

    public void Load(ModeParameters parameters)
    {
        var battleManager = new BattleManager(_battleRegistry, parameters.GetValue<BattleConfig>("config"), _chatView);
        Task.Run(battleManager.DoRound);
    }
}