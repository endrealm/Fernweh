using System;
using System.Collections.Generic;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl;

namespace Core.States;

public class RenderContext
{
    private readonly IStateManager _stateManager;
    private readonly GameManager _gameManager;
    public string PreviousStateId { get; }
    public string ActiveStateId { get; }

    public RenderContext(IStateManager stateManager, GameManager gameManager, string previousStateId, string activeStateId)
    {
        _stateManager = stateManager;
        _gameManager = gameManager;
        ActiveStateId = activeStateId;
        PreviousStateId = previousStateId;
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
    
    public void StartBattle(string victoryState = "null", string looseState = "null")
    {
        var config = new BattleConfig(
            new List<string> {"test","test","test","test","test","test",}
            );
        _gameManager.LoadMode("battle", new ModeParameters()
            .AppendData("victoryState", victoryState)
            .AppendData("looseState", looseState)
            .AppendData("config", config));
    }
    public void Exit()
    {
        _stateManager.LoadState("null");
    }
}