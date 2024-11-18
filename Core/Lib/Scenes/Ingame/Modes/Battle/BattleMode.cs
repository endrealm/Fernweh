using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleMode : IMode
{
    private readonly BattleRegistry _battleRegistry;

    private readonly BattleChatView _chatView;
    private readonly GameManager _gameManager;
    private readonly BattleGameView _gameView;

    private readonly ISoundPlayer _soundPlayer;

    private Dictionary<string, object> _snapshot = new();

    public BattleMode(
        GameManager gameManager,
        ISpriteManager spriteManager,
        BattleRegistry battleRegistry,
        ILocalizationManager localizationManager,
        IFontManager fontManager,
        ISoundPlayer soundPlayer
    )
    {
        _gameManager = gameManager;
        _battleRegistry = battleRegistry;
        _soundPlayer = soundPlayer;

        _chatView = new BattleChatView(localizationManager, fontManager);
        _gameView = new BattleGameView(spriteManager, fontManager, localizationManager);
    }

    public Color Background { get; } = new(18, 14, 18);
    public IChatView ChatView => _chatView;

    public IGameView GameView => _gameView;

    public void Load(ModeParameters parameters)
    {
        _soundPlayer.PlaySong("battle");

        var config = parameters.GetValue<BattleConfig>("config");
        var victoryState = parameters.GetValue<string>("victoryState");
        var looseState = parameters.GetValue<string>("looseState");
        var background = parameters.GetValue<string>("background");

        _snapshot = new Dictionary<string, object>
        {
            {"victoryState", victoryState},
            {"looseState", looseState},
            {"config", JsonConvert.SerializeObject(config)},
            {"background", background}
        };

        var battleManager = new BattleManager(ChatView, _battleRegistry, config, _chatView,
            () => LoadOverwoldState(victoryState),
            () => LoadOverwoldState(looseState),
            _gameManager.EventHandler,
            _soundPlayer
        );
        _chatView.BattleManager = battleManager;
        _gameView.LoadBattle(battleManager, background);
        Task.Run(battleManager.DoRound);
    }

    public void Load(Dictionary<string, object> data)
    {
        Load(new ModeParameters()
            .AppendData("victoryState", data["victoryState"])
            .AppendData("looseState", data["looseState"])
            .AppendData("config", JsonConvert.DeserializeObject<BattleConfig>((string) data["config"]))
            .AppendData("background", data["background"])
        );
    }

    public void Save(Dictionary<string, object> data)
    {
        foreach (var pair in _snapshot) data.Add(pair.Key, pair.Value);
    }

    private void LoadOverwoldState(string state)
    {
        _gameManager.StateManager.weakNextID = state;
        _gameManager.LoadMode("overworld", new ModeParameters().AppendData("state", "post_battle_overview"));
    }
}