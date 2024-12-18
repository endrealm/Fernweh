﻿using System;
using System.Collections.Generic;
using Core.Content;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Views;
using Core.States;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Core.Scenes.Ingame.Modes.Overworld;

public class OverworldMode : IMode, IStateManager
{
    private readonly StateChatView _chatView;
    private readonly IFontManager _fontManager;
    private readonly GameManager _gameManager;
    private readonly ILocalizationManager _localizationManager;
    private readonly ISaveSystem _saveSystem;
    private readonly ISoundPlayer _soundPlayer;
    private readonly StateRegistry _stateRegistry;
    private string _weakNextId;
    public WorldGameView worldGameView;

    public OverworldMode(GameManager gameManager, IGlobalEventHandler eventHandler, ContentRegistry content,
        StateRegistry stateRegistry,
        IFontManager fontManager, ILocalizationManager localizationManager, ISaveSystem saveSystem,
        ISoundPlayer soundPlayer)
    {
        _gameManager = gameManager;
        _stateRegistry = stateRegistry;
        _fontManager = fontManager;
        _localizationManager = localizationManager;
        _saveSystem = saveSystem;
        _soundPlayer = soundPlayer;
        _chatView = new StateChatView(localizationManager, fontManager);
        worldGameView = new WorldGameView(eventHandler, this, soundPlayer, content);
        GameView = worldGameView;
        ActiveState = _stateRegistry.ReadState("null"); // Start with "null" state.
        StateChangedEvent += OnStateChanged;
    }

    private string LastSaveStateWeak { get; set; }
    private string LastSaveState { get; set; }

    public Color Background { get; private set; }

    public IChatView ChatView => _chatView;

    public IGameView GameView { get; }

    public void Load(ModeParameters parameters)
    {
        _soundPlayer.PlaySong("overworld");

        if (parameters.HasKey("state")) LoadState(parameters.GetValue<string>("state"));
    }

    public void Load(Dictionary<string, object> data)
    {
        weakNextID = (string) data["WeakState"];
        Load(new ModeParameters().AppendData("state", (string) data["State"] ?? "null"));
        if (data.ContainsKey("LoadedMap"))
            worldGameView.MapDataRegistry.LoadMap((string) data["LoadedMap"]);
        if (data.ContainsKey("PlayerPosX") && data.ContainsKey("PlayerPosY"))
            worldGameView.Player.TeleportPlayer(new Vector2((long) data["PlayerPosX"], (long) data["PlayerPosY"]));
        if (data.TryGetValue("DiscoveredTiles", out var value))
            worldGameView.DiscoveredTiles =
                JsonConvert.DeserializeObject<Dictionary<string, List<Vector2>>>((string) value);
    }

    public void Save(Dictionary<string, object> data)
    {
        data.Add("State", LastSaveState);
        data.Add("WeakState", LastSaveStateWeak);
        data.Add("LoadedMap", worldGameView.MapDataRegistry.GetLoadedMapName());
        data.Add("PlayerPosX", (long) worldGameView.Player.CurrentPos.X / 32);
        data.Add("PlayerPosY", (long) worldGameView.Player.CurrentPos.Y / 32);
        data.Add("DiscoveredTiles", JsonConvert.SerializeObject(worldGameView.DiscoveredTiles));
    }

    public string weakNextID
    {
        get => _weakNextId;
        set
        {
            if (ActiveState.AllowSave) LastSaveStateWeak = value;
            _weakNextId = value;
        }
    }

    public IState ActiveState { get; private set; }

    public void LoadState(string stateId)
    {
        var oldState = ActiveState;

        if ((stateId == null || stateId == "null") && weakNextID != null)
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
        StateChangedEvent?.Invoke(new StateChangedEventArgs
        {
            OldState = oldState,
            NewState = ActiveState
        });
    }

    public event StateChangedEventHandler StateChangedEvent;

    private void OnStateChanged(StateChangedEventArgs args)
    {
        var renderer = new StateRenderer(_localizationManager, _fontManager, color => Background = color,
            args.OldState.ClearScreenPost);
        var context = new RenderContext(this, _gameManager, args.OldState.Id, args.NewState.Id);
        _stateRegistry.GlobalEventHandler.EmitPreStateRenderEvent(renderer, context);
        args.NewState.Render(renderer, context);
        _stateRegistry.GlobalEventHandler.EmitPostStateRenderEvent(renderer, context);
        context.IsRunning = false;
        _chatView.RenderResults(renderer, args.NewState.Sticky);
        if (context.ExitEarly) _gameManager.StateManager.LoadState("null");
    }
}

public delegate void StateChangedEventHandler(StateChangedEventArgs args);

public class StateChangedEventArgs : EventArgs
{
    public IState OldState { get; set; }
    public IState NewState { get; set; }
}