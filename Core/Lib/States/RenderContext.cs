using System;
using System.Collections.Generic;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl;
using Core.Scenes.Ingame.Views;
using NLua;
using Microsoft.Xna.Framework;

namespace Core.States;

public class RenderContext
{
    private readonly IStateManager _stateManager;
    private readonly GameManager _gameManager;
    private WorldGameView _worldGameView;
    public string PreviousStateId { get; }
    public string ActiveStateId { get; }
    public bool IsRunning { get; set; } = true;
    public bool ExitEarly { get; private set; }

    public RenderContext(IStateManager stateManager, GameManager gameManager, string previousStateId, string activeStateId)
    {
        _stateManager = stateManager;
        _gameManager = gameManager;
        ActiveStateId = activeStateId;
        PreviousStateId = previousStateId;
        _worldGameView = _gameManager.GetOverworldMode().worldGameView;
    }

    public void ChangeState(string stateId)
    {
        try
        {
            _stateManager.LoadState(stateId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }

    public void StartBattle(LuaTable enemies, string background, string victoryState = "null", string looseState = "null")
    {
        List<string> enemiesString = new();
        foreach (var item in enemies.Values)
            enemiesString.Add(item.ToString());

        var config = new BattleConfig(enemiesString);
        _gameManager.LoadMode("battle", new ModeParameters()
            .AppendData("victoryState", victoryState)
            .AppendData("looseState", looseState)
            .AppendData("config", config)
            .AppendData("background", background));
    }

    public void Exit()
    {
        if (IsRunning)
        {
            ExitEarly = true;
            return;
        }
        _stateManager.LoadState("null");
    }

    public void MovePlayer(int x, int y)
    {
        _worldGameView.player.MovePlayer(new Vector2(x, y));
    }

    public void TeleportPlayer(int x, int y)
    {
        _worldGameView.player.TeleportPlayer(new Vector2(x,y));
    }

    public void LoadMap(string name)
    {
        _worldGameView.mapDataRegistry.LoadMap(name);
    }

    public string GetLoadedMap()
    {
        return _worldGameView.mapDataRegistry.GetLoadedMapName();
    }

    public void PlaySFX(string name)
    {
        _gameManager.SoundPlayer.PlaySFX(name);
    }

    public void PlaySong(string name)
    {
        _gameManager.SoundPlayer.PlaySong(name);
    }
}