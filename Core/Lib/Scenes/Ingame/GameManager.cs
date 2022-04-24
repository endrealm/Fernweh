using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Modes.Battle;
using Core.Scenes.Ingame.Modes.Battle.Impl;
using Core.Content;
using Core.Saving;
using Core.States;
using Core.Utils;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class GameManager: ILoadable
{
    private readonly IGameSave _gameSave;
    private readonly ISaveSystem _saveSystem;
    public IMode Mode { get; private set; }
    public IStateManager StateManager { get; set; }
    
    private Dictionary<string, IMode> _modes = new();
    private DynamicBattleSpriteManager _spriteManager = new ();
    private readonly IGlobalEventHandler _eventHandler;
    private const string SaveKey = "_internal:Mode";


    public GameManager(BattleRegistry registry, StateRegistry stateRegistry, IFontManager fontManager, DialogTranslationData translationData, IGameSave gameSave, ISaveSystem saveSystem)
    {
        _gameSave = gameSave;
        _saveSystem = saveSystem;
        _eventHandler = stateRegistry.GlobalEventHandler;
        var overworld = new OverworldMode(this, stateRegistry.GlobalEventHandler, stateRegistry, fontManager, translationData, saveSystem);
        StateManager = overworld;
        _modes.Add("overworld", overworld);
        _modes.Add("battle", new BattleMode(this, _spriteManager, registry, translationData, fontManager));

        LoadGameState();
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

    public void Load(ContentLoader content)
    {
        _spriteManager.Load(content, _eventHandler);
        foreach (var mode in _modes.Values)
        {
            mode.ChatView.Load(content);
            mode.GameView.Load(content);
        }
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
    
    private void LoadGameState()
    {
        if (!_gameSave.Data.ContainsKey(SaveKey))
        {
            LoadMode("overworld", new ModeParameters());
            return;
        }

        var data = (Dictionary<string, object>) _gameSave.Data[SaveKey];
        Mode = _modes[(string) data["Mode"]];
        Mode.Load((Dictionary<string, object>) data["Data"]);
    }

    private string ActiveModeKey()
    {
        foreach (var keyValuePair in _modes)
        {
            if (keyValuePair.Value == Mode) return keyValuePair.Key;
        }

        throw new Exception("Invalid mode");
    }
}
