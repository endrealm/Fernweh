using System;
using System.Collections.Generic;
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
        LoadMode("overworld");
    }

    public void LoadMode(string id)
    {
        Mode = _modes[id];
    }

    public void Load(ContentManager content)
    {
        foreach (var mode in _modes.Values)
        {
            mode.ChatView.Load(content);
            mode.GameView.Load(content);
        }
    }
}
