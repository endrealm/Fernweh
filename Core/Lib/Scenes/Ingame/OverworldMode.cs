using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Localization;
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
    private readonly ILocalizationManager _localizationManager;
    private readonly ISaveSystem _saveSystem;
    private readonly StateChatView _chatView;
    public WorldGameView worldGameView;
    private string _weakNextId;

    public string weakNextID
    {
        get => _weakNextId;
        set
        {
            if (ActiveState.AllowSave) LastSaveStateWeak = value;
            _weakNextId = value;
        }
    }

    private string LastSaveStateWeak { get;  set; }
    private string LastSaveState { get;  set; }

    public OverworldMode(GameManager gameManager, IGlobalEventHandler eventHandler, StateRegistry stateRegistry,
        IFontManager fontManager, ILocalizationManager localizationManager, ISaveSystem saveSystem)
    {
        _gameManager = gameManager;
        _stateRegistry = stateRegistry;
        _fontManager = fontManager;
        _localizationManager = localizationManager;
        _saveSystem = saveSystem;
        _chatView = new StateChatView(localizationManager, fontManager);
        worldGameView = new WorldGameView(eventHandler, this);
        GameView = worldGameView;
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
        Load(new ModeParameters().AppendData("state", (string) data["State"] ?? "null"));
        if(data.ContainsKey("PlayerPosX") && data.ContainsKey("PlayerPosY"))
            worldGameView.player.TeleportPlayer(new Vector2(Int32.Parse((string)data["PlayerPosX"]), Int32.Parse((string)data["PlayerPosY"])));
    }

    public void Save(Dictionary<string, object> data)
    {
        data.Add("State", LastSaveState);
        data.Add("WeakState", LastSaveStateWeak);
        data.Add("PlayerPosX", (worldGameView.player.CurrentPos.X / 32).ToString());
        data.Add("PlayerPosY", (worldGameView.player.CurrentPos.Y / 32).ToString());
    }

    public event StateChangedEventHandler StateChangedEvent;
    public IState ActiveState { get; private set; }
    
    public void LoadState(string stateId)
    {
        var oldState = ActiveState;
        

        if (stateId == "null" && weakNextID != null)
        {
            var weak = weakNextID;
            weakNextID = null; // Clear before saved in next render
            LoadState(weak);
            return;
        }
        ActiveState = _stateRegistry.ReadState(stateId);

        if (ActiveState.AllowSave)
        {
            LastSaveState = ActiveState.Id;
            _saveSystem.SaveAll();
        }
        _stateRegistry.GlobalEventHandler.EmitPreStateChangeEvent();
        StateChangedEvent?.Invoke(new StateChangedEventArgs()
        {
            OldState = oldState,
            NewState = ActiveState
        });
    }
    
    private void OnStateChanged(StateChangedEventArgs args)
    {
        var renderer = new StateRenderer(_localizationManager, Language.EN_US, _fontManager, (color) => Background = color, args.OldState.ClearScreenPost);
        var context = new RenderContext(this, _gameManager, args.OldState.Id, args.NewState.Id);
        _stateRegistry.GlobalEventHandler.EmitPreStateRenderEvent(renderer, context);
        args.NewState.Render(renderer, context);
        _stateRegistry.GlobalEventHandler.EmitPostStateRenderEvent(renderer, context);
        _chatView.RenderResults(renderer, args.NewState.Sticky);
    }

}

public delegate void StateChangedEventHandler(StateChangedEventArgs args);

public class StateChangedEventArgs : EventArgs
{
    public IState OldState { get; set; }
    public IState NewState { get; set; }
}