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
    
    public void StartBattle(string victoryState = "null", string looseState = "null")
    {
        var config = new BattleConfig(
            new List<string> {"test","test","test","test","test","test",}, 
            new List<ParticipantConfig>
            {
                new ParticipantConfigBuilder("Yennifer")
                    .Health(1000)
                    .Mana(1000)
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability").Build())
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability_2").Build())
                    .Build(),
                new ParticipantConfigBuilder("Triss")
                    .Health(1000)
                    .Mana(1000)
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability").Build())
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability_2").Build())
                    .Build(),
                new ParticipantConfigBuilder("Geralt")
                    .Health(1000)
                    .Mana(1000)
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability").Build())
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability_2").Build())
                    .Build(),
                new ParticipantConfigBuilder("Ciri")
                    .Health(1000)
                    .Mana(1000)
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability").Build())
                    .AddAbility(new LuaAbilityConfigBuilder("test_ability_2").Build())
                    .Build(),
            }
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