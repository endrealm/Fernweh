using System;
using Core.Scenes.Ingame.Modes;
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
    private readonly StateChatView _chatView;

    public OverworldMode(GameManager gameManager, IGlobalEventHandler eventHandler, StateRegistry stateRegistry, IFontManager fontManager, DialogTranslationData translationData)
    {
        _gameManager = gameManager;
        _stateRegistry = stateRegistry;
        _fontManager = fontManager;
        _translationData = translationData;
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

    public event StateChangedEventHandler StateChangedEvent;
    public IState ActiveState { get; private set; }
    
    public void LoadState(string stateId)
    {
        _stateRegistry.GlobalEventHandler.EmitPreStateChangeEvent();
        ActiveState = _stateRegistry.ReadState(stateId);
        StateChangedEvent?.Invoke(new StateChangedEventArgs()
        {
            NewState = ActiveState
        });
    }
    
    private void OnStateChanged(StateChangedEventArgs args)
    {
        var renderer = new StateRenderer(_translationData, Language.EN_US, _fontManager, (color) => Background = color);
        args.NewState.Render(renderer, new RenderContext(this, _gameManager));
        _chatView.RenderResults(renderer);
    }

}

public delegate void StateChangedEventHandler(StateChangedEventArgs args);

public class StateChangedEventArgs : EventArgs
{
    public IState NewState { get; set; }
}