using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Views;
using Core.States;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class OverworldMode: IMode, IStateManager
{
    private readonly GameManager _gameManager;
    private readonly StateRegistry _stateRegistry;
    private readonly IFontManager _fontManager;
    private readonly DialogTranslationData _translationData;
    private readonly ISaveSystem _saveSystem;
    private readonly StateChatView _chatView;
    public string weakNextID { get;  set; }

    public OverworldMode(GameManager gameManager, IGlobalEventHandler eventHandler, StateRegistry stateRegistry,
        IFontManager fontManager, DialogTranslationData translationData, ISaveSystem saveSystem)
    {
        _gameManager = gameManager;
        _stateRegistry = stateRegistry;
        _fontManager = fontManager;
        _translationData = translationData;
        _saveSystem = saveSystem;
        _chatView = new StateChatView(translationData, fontManager);
        GameView = new WorldGameView(eventHandler, this);
        ActiveState = _stateRegistry.ReadState("null"); // Start with "null" state.
        StateChangedEvent += OnStateChanged;
    }

    public Color Background { get; private set; }

    public IChatView ChatView => _chatView;

    public IGameView GameView { get; }
    public void Load(ModeParameters parameters)
    {
        if(parameters.HasKey("state"))
        {
            LoadState(parameters.GetValue<string>("state"));
        }
    }

    public void Load(Dictionary<string, object> data)
    {
        weakNextID = (string) data["WeakState"];
        Load(new ModeParameters().AppendData("state", (string) data["State"]));
    }

    public void Save(Dictionary<string, object> data)
    {
        data.Add("State", ActiveState.Id);
        data.Add("WeakState", weakNextID);
    }

    public event StateChangedEventHandler StateChangedEvent;
    public IState ActiveState { get; private set; }
    
    public void LoadState(string stateId)
    {
        _stateRegistry.GlobalEventHandler.EmitPreStateChangeEvent();
        var oldState = ActiveState;
        ActiveState = _stateRegistry.ReadState(stateId);
        StateChangedEvent?.Invoke(new StateChangedEventArgs()
        {
            OldState = oldState,
            NewState = ActiveState
        });

        if (stateId == "null" && weakNextID != null)
        {
            LoadState(weakNextID);
            weakNextID = null;
        }
        _saveSystem.SaveAll();
    }
    
    private void OnStateChanged(StateChangedEventArgs args)
    {
        var renderer = new StateRenderer(_translationData, Language.EN_US, _fontManager, (color) => Background = color);
        var context = new RenderContext(this, _gameManager, args.OldState.Id, args.NewState.Id);
        args.NewState.Render(renderer, context);
        _stateRegistry.GlobalEventHandler.EmitPostStateChangeEvent(renderer, context);
        _chatView.RenderResults(renderer);
    }

}

public delegate void StateChangedEventHandler(StateChangedEventArgs args);

public class StateChangedEventArgs : EventArgs
{
    public IState OldState { get; set; }
    public IState NewState { get; set; }
}