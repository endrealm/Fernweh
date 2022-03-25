﻿using System.Collections.Generic;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl;

namespace Core.States;

public class RenderContext
{
    private readonly IStateManager _stateManager;
    private readonly GameManager _gameManager;

    public RenderContext(IStateManager stateManager, GameManager gameManager)
    {
        _stateManager = stateManager;
        _gameManager = gameManager;
    }

    public void ChangeState(string stateId)
    {
        _stateManager.LoadState(stateId);
    }
    
    public void StartBattle()
    {
        var config = new BattleConfig(
            new List<string> {"test"}, 
            new List<ParticipantConfig>
            {
                new ParticipantConfigBuilder("you")
                    .Health(10)
                    .Mana(10)
                    .Build()
            }
            );
        _gameManager.LoadMode("battle", new ModeParameters().AppendData("config", config));
    }
    public void Exit()
    {
        _stateManager.LoadState("null");
    }
}