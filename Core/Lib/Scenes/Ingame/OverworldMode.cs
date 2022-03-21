using System;
using Core.States;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class OverworldMode: IMode, IStateManager
{
    private readonly StateRegistry _stateRegistry;
    private readonly IFontManager _fontManager;
    private readonly DialogTranslationData _translationData;

    public OverworldMode(IGlobalEventHandler eventHandler, StateRegistry stateRegistry, IFontManager fontManager, DialogTranslationData translationData)
    {
        _stateRegistry = stateRegistry;
        _fontManager = fontManager;
        _translationData = translationData;
        ChatView = new ChatView();
        GameView = new GameView(eventHandler, this);
        ActiveState = _stateRegistry.ReadState("null"); // Start with "null" state.
        StateChangedEvent += OnStateChanged;
    }

    public Color Background { get; private set; }
    public ChatView ChatView { get; }
    public GameView GameView { get; }
    
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
        args.NewState.Render(renderer, new RenderContext(this));
        ChatView.RenderResults(renderer);
    }

}

public delegate void StateChangedEventHandler(StateChangedEventArgs args);

public class StateChangedEventArgs : EventArgs
{
    public IState NewState { get; set; }
}