﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle.Impl;
using Core.Scenes.Ingame.Modes;
using Core.Utils;

namespace Core.Scenes.Ingame.Battle;

public class BattleManager
{
    private readonly IChatView _chatView;
    private readonly BattleRegistry _registry;
    private readonly IPlayerBattleInput _playerInput;
    private readonly List<IBattleParticipant> _friendlies;
    private readonly List<IBattleParticipant> _enemies;

    public BattleManager(IChatView chatView, BattleRegistry registry, BattleConfig config, IPlayerBattleInput playerInput)
    {
        _chatView = chatView;
        _registry = registry;
        _playerInput = playerInput;
        _enemies = config.Enemies.Select(id => CreateParticipant(registry.GetParticipantFactory(id).Produce()))
            .ToList();
        _friendlies = config.Friendlies.Select(CreateParticipant).ToList();
    }

    public List<IBattleParticipant> Enemies => _enemies;
    public List<IBattleParticipant> Friendlies => _friendlies;
    public List<IBattleParticipant> All
    {
        get
        {
            var all = new List<IBattleParticipant>(_enemies);
            all.AddRange(_friendlies);
            return all;
        }
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

        var actionQueue = actions.ToStack();
        while (actionQueue.Count > 0)
        {
            var action = actionQueue.Pop();
            
            // skip dead participants
            if (!action.AllowDeath && action.Participant is { State: ParticipantState.Dead })
            {
                continue;
            }
            
            var context = new ActionContext(_chatView);
            await action.DoAction(context);
            actionQueue.AddRange(context.GetActionList());
            
            // Check for any changed states
            _friendlies.ForEach(participant => participant.UpdateParticipantState());
            _enemies.ForEach(participant => participant.UpdateParticipantState());
        }
        
        // Execute and await all actions
        _friendlies.ForEach(participant => { participant.OnTurnEnd(); });
        _enemies.ForEach(participant => { participant.OnTurnEnd(); });
        await Task.Delay(2000);

        Task.Run(DoRound);
    }
    
}