using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleMode : IMode
{

    private readonly BattleChatView _chatView;
    public Color Background { get; } = new(18, 14, 18);
    public IChatView ChatView => _chatView;

    public IGameView GameView => _gameView;

    private readonly GameManager _gameManager;
    private readonly BattleRegistry _battleRegistry;
    private readonly BattleGameView _gameView;

    private Dictionary<string, object> _snapshot = new();

    public BattleMode(
        GameManager gameManager,
        IBattleSpriteManager spriteManager, 
        BattleRegistry battleRegistry, 
        ILocalizationManager localizationManager, 
        IFontManager fontManager
    ) {
        _gameManager = gameManager;
        _battleRegistry = battleRegistry;

        _chatView = new BattleChatView(localizationManager, fontManager);
        _gameView = new BattleGameView(spriteManager, fontManager, localizationManager);
    }

    public void Load(ModeParameters parameters)
    {
        var config = parameters.GetValue<BattleConfig>("config");
        var victoryState = parameters.GetValue<string>("victoryState");
        var looseState = parameters.GetValue<string>("looseState");

        _snapshot = new Dictionary<string, object>
        {
            {"victoryState", victoryState},
            {"looseState", looseState},
            {"config", JsonConvert.SerializeObject(config)},
        };

        var battleManager = new BattleManager(ChatView, _battleRegistry, config, _chatView, 
            () => LoadOverwoldState(victoryState),
            () => LoadOverwoldState(looseState),
            _gameManager.EventHandler
        );
        _chatView.BattleManager = battleManager;
        _gameView.LoadBattle(battleManager);
        Task.Run(battleManager.DoRound);
    }

    public void Load(Dictionary<string, object> data)
    {
        Load(new ModeParameters()
            .AppendData("victoryState", data["victoryState"])
            .AppendData("looseState", data["looseState"])
            .AppendData("config", JsonConvert.DeserializeObject<BattleConfig>((string) data["config"]))
        );
    }

    public void Save(Dictionary<string, object> data)
    {
        foreach (var pair in _snapshot)
        {
            data.Add(pair.Key, pair.Value);
        }
    }

    private void LoadOverwoldState(string state)
    {
        _gameManager.LoadMode("overworld", new ModeParameters().AppendData("state", state));
    }
}