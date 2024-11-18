using System;
using System.Collections.Generic;
using Core.Content;
using Core.Saving;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Modes.Battle;
using Core.Scenes.Ingame.Modes.Overworld;
using Core.States;
using Core.Utils;

namespace Core.Scenes.Ingame;

public class GameManager : ILoadable
{
    private const string SaveKey = "_internal:Mode";

    private readonly ContentRegistry _contentRegistry;
    private readonly IGameSave _gameSave;

    private readonly Dictionary<string, IMode> _modes = new();
    private readonly ISaveSystem _saveSystem;
    private readonly StateRegistry _stateRegistry;

    public GameManager(ContentRegistry content, BattleRegistry registry, StateRegistry stateRegistry,
        IFontManager fontManager, ILocalizationManager localizationManager, IGameSave gameSave, ISaveSystem saveSystem)
    {
        _contentRegistry = content;
        _stateRegistry = stateRegistry;
        _gameSave = gameSave;
        _saveSystem = saveSystem;
        EventHandler = stateRegistry.GlobalEventHandler;
        SoundPlayer = content.soundRegistry;
        var overworld = new OverworldMode(this, stateRegistry.GlobalEventHandler, _contentRegistry, stateRegistry,
            fontManager, localizationManager, saveSystem, SoundPlayer);
        StateManager = overworld;
        _modes.Add("overworld", overworld);
        _modes.Add("battle",
            new BattleMode(this, content.dynamicBattleSpriteManager, registry, localizationManager, fontManager,
                SoundPlayer));
        LoadMode("overworld", new ModeParameters());
    }

    public IMode Mode { get; private set; }
    public IStateManager StateManager { get; set; }

    public ISoundPlayer SoundPlayer { get; }

    //private DynamicBattleSpriteManager _spriteManager = new ();
    public IGlobalEventHandler EventHandler { get; }

    public void Load(ContentLoader content)
    {
        //_spriteManager.Load(_contentRegistry, EventHandler);
        foreach (var mode in _modes.Values)
        {
            mode.ChatView.Load(content);
            mode.GameView.Load(content);
        }
    }

    public void LoadState(string stateId)
    {
        LoadMode(stateId, new ModeParameters());
    }

    public void LoadMode(string id, ModeParameters parameters)
    {
        Mode = _modes[id];
        Mode.Load(parameters);
        _saveSystem.SaveAll();
    }

    public void Save()
    {
        var data = new Dictionary<string, object>();

        var dict = new Dictionary<string, object>
        {
            {"Mode", ActiveModeKey()},
            {"Data", data}
        };

        Mode.Save(data);
        _gameSave.Data.Remove(SaveKey);
        _gameSave.Data.Add(SaveKey, dict);
    }

    public void LoadGameState()
    {
        if (!_gameSave.Data.ContainsKey(SaveKey))
        {
            LoadMode("overworld", new ModeParameters().AppendData("state", _stateRegistry.EntryState));
            return;
        }

        var data = (Dictionary<string, object>) _gameSave.Data[SaveKey];
        Mode = _modes[(string) data["Mode"]];
        Mode.Load((Dictionary<string, object>) data["Data"]);
    }

    public OverworldMode GetOverworldMode()
    {
        return _modes["overworld"] as OverworldMode;
    }

    private string ActiveModeKey()
    {
        foreach (var keyValuePair in _modes)
            if (keyValuePair.Value == Mode)
                return keyValuePair.Key;

        throw new Exception("Invalid mode");
    }
}