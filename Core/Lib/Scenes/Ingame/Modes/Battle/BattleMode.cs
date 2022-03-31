using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleMode : IMode
{

    private readonly BattleChatView _chatView;
    public Color Background { get; } = new(18, 14, 18);
    public IChatView ChatView => _chatView;

    public IGameView GameView => _gameView;

    private readonly BattleRegistry _battleRegistry;
    private readonly BattleGameView _gameView;

    public BattleMode(IBattleSpriteManager spriteManager, BattleRegistry battleRegistry, DialogTranslationData translationData, IFontManager fontManager)
    {
        _battleRegistry = battleRegistry;

        _chatView = new BattleChatView(translationData, fontManager);
        _gameView = new BattleGameView(spriteManager, fontManager, translationData);
    }

    public void Load(ModeParameters parameters)
    {
        var battleManager = new BattleManager(ChatView, _battleRegistry, parameters.GetValue<BattleConfig>("config"), _chatView);
        _chatView.BattleManager = battleManager;
        _gameView.LoadBattle(battleManager);
        Task.Run(battleManager.DoRound);
    }
}