using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Modes.Battle;
using Core.Scenes.Ingame.Modes.Battle.Impl;
using System.Collections.Generic;
using Core.Content;
using Core.States;
using Core.Utils;
using Microsoft.Xna.Framework.Content;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class GameManager: ILoadable
{
    public IMode Mode { get; private set; }
    public IStateManager StateManager { get; set; }
    
    private Dictionary<string, IMode> _modes = new();
    private StaticBattleSpriteManager _spriteManager = new StaticBattleSpriteManager();

    public GameManager(BattleRegistry registry, StateRegistry stateRegistry, IFontManager fontManager, DialogTranslationData translationData)
    {
        var overworld = new OverworldMode(this, stateRegistry.GlobalEventHandler, stateRegistry, fontManager, translationData);
        StateManager = overworld;
        _modes.Add("overworld", overworld);
        _modes.Add("battle", new BattleMode(this, _spriteManager, registry, translationData, fontManager));
        LoadMode("overworld");
    }

    public void LoadState(string stateId)
    {
        LoadMode(id, new ModeParameters());
    }
    
    public void LoadMode(string id, ModeParameters parameters)
    {
        Mode = _modes[id];
        Mode.Load(parameters);
    }

    public void Load(ContentLoader content)
    {
        foreach (var mode in _modes.Values)
        {
            mode.ChatView.Load(content);
            mode.GameView.Load(content);
        }
    }
}
