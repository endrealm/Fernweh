using System;
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

    public GameManager(StateRegistry stateRegistry, IFontManager fontManager, DialogTranslationData translationData)
    {
        var overworld = new OverworldMode(stateRegistry.GlobalEventHandler, stateRegistry, fontManager, translationData);
        StateManager = overworld;
        _modes.Add("overworld", overworld);
        _modes.Add("battle", new BattleMode());
        LoadMode("overworld");
    }

    public void LoadMode(string id)
    {
        Mode = _modes[id];
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
