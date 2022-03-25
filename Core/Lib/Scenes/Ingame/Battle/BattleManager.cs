﻿using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Battle.Impl;
using Core.Utils;

namespace Core.Scenes.Ingame.Battle;

public class BattleManager
{
    private readonly BattleRegistry _registry;
    private readonly IPlayerBattleInput _playerInput;
    private readonly List<IBattleParticipant> _friendlies;
    private readonly List<IBattleParticipant> _enemies;

    public BattleManager(BattleRegistry registry, BattleConfig config, IPlayerBattleInput playerInput)
    {
        _registry = registry;
        _playerInput = playerInput;
        _enemies = config.Enemies.Select(id => CreateParticipant(registry.GetParticipantFactory(id).Produce()))
            .ToList();
        _friendlies = config.Friendlies.Select(CreateParticipant).ToList();
    }

    private IBattleParticipant CreateParticipant(ParticipantConfig config)
    {
        var participant = new BasicParticipant(config.Id, config);
        config.Abilities.ForEach(abilityConfig =>
        {
            var ability = _registry.GetAbilityFactory(abilityConfig.Id).Produce(abilityConfig);
            participant.GetAbilities().Add(ability);
        });
        return participant;
    }

    public async void DoRound()
    {
        // Assigns actions to all player controlled units
        await _playerInput.HandlePlayerInput(_friendlies);
        var actions = new List<IBattleAction>();
        _friendlies.ForEach(participant =>
        {
            participant.OnNextTurn(out var skip);
            if (skip) return;
            participant.NextAction ??= participant.Strategy.SelectAction(participant);
            actions.Add(participant.NextAction);
            participant.NextAction = null;
        });
        _enemies.ForEach(participant =>
        {
            participant.OnNextTurn(out var skip);
            if (skip) return;
            participant.NextAction ??= participant.Strategy.SelectAction(participant);
            actions.Add(participant.NextAction);
            participant.NextAction = null;
        });

        // Sort by priority
        actions.Sort((act1, act2) => act1.Priority - act2.Priority);

        var actionQueue = actions.ToQueue();
        while (actionQueue.Count > 0)
        {
            var action = actionQueue.Dequeue();
            var context = new ActionContext();
            await action.DoAction(context);
            actionQueue.AddRange(context.GetActionList());
        }
        
        // Execute and await all actions
        _friendlies.ForEach(participant => { participant.OnTurnEnd(); });
        _enemies.ForEach(participant => { participant.OnTurnEnd(); });
    }
    
}