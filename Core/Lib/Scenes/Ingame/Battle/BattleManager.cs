﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle.Impl;
using Core.Scenes.Ingame.Views;
using Core.States;
using Core.Utils;

namespace Core.Scenes.Ingame.Battle;

public class BattleManager
{
    private readonly IChatView _chatView;
    private readonly IGlobalEventHandler _globalEventManager;
    private readonly Action _onLoose;
    private readonly Action _onWin;
    private readonly IPlayerBattleInput _playerInput;
    private readonly Random _random = new();
    private readonly ISoundPlayer _soundPlayer;

    public BattleManager(IChatView chatView, BattleRegistry registry, BattleConfig config,
        IPlayerBattleInput playerInput, Action onWin, Action onLoose, IGlobalEventHandler globalEventManager,
        ISoundPlayer soundPlayer)
    {
        _chatView = chatView;
        Registry = registry;
        _playerInput = playerInput;
        _onWin = onWin;
        _onLoose = onLoose;
        _globalEventManager = globalEventManager;
        _soundPlayer = soundPlayer;
        var enemyDict = new Dictionary<string, int>();
        Enemies = config.Enemies
            .Select(id =>
            {
                if (enemyDict.TryGetValue(id, out var index))
                    enemyDict[id] = ++index;
                else
                    enemyDict.Add(id, ++index);

                return CreateParticipant(id + " " + index, registry.GetParticipantFactory(id).Produce());
            })
            .ToList();
        Friendlies = Registry.FriendlyParticipantsProvider.Load()
            .Select(config1 => CreateParticipant(config1.Id, config1)).ToList();
    }

    public BattleRegistry Registry { get; }

    public List<IBattleParticipant> Enemies { get; }

    public List<IBattleParticipant> Friendlies { get; }

    public List<IBattleParticipant> All
    {
        get
        {
            var all = new List<IBattleParticipant>(Enemies);
            all.AddRange(Friendlies);
            return all;
        }
    }

    private IBattleParticipant CreateParticipant(string particpantId, ParticipantConfig config)
    {
        var participant = new BasicParticipant(particpantId, config.Id, config);
        config.Abilities.ForEach(abilityConfig =>
        {
            var ability = Registry.GetAbilityFactory(abilityConfig.Id).Produce(abilityConfig);
            participant.GetAbilities().Add(ability);
        });
        return participant;
    }

    public async void DoRound()
    {
        await Task.Delay(30); // prevents bug where numerous battles in a row crash the game

        if (CheckForEndCondition()) return;

        // Assigns actions to all player controlled units
        await _playerInput.HandlePlayerInput(Friendlies.Where(x => x.State == ParticipantState.Alive).ToList());
        var actions = new List<IBattleAction>();
        Friendlies.ForEach(participant =>
        {
            participant.OnNextTurn(out var skip);
            if (skip) return;
            participant.NextAction ??= participant.Strategy.SelectAction(_random, this, participant);
            actions.Add(participant.NextAction);
            participant.NextAction = null;
        });
        Enemies.ForEach(participant =>
        {
            participant.OnNextTurn(out var skip);
            if (skip) return;
            participant.NextAction ??= participant.Strategy.SelectAction(_random, this, participant);
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
            if (!action.AllowDeath && action.Participant is {State: ParticipantState.Dead}) continue;

            var context = new ActionContext(_chatView, _soundPlayer);
            await action.DoAction(context);
            actionQueue.AddRange(context.GetActionList());

            if (action.CausesStateCheck)
            {
                // Check for any changed states
                var updateContext = new ActionContext(_chatView, _soundPlayer);
                Friendlies.ForEach(participant => participant.UpdateParticipantState(updateContext));
                Enemies.ForEach(participant => participant.UpdateParticipantState(updateContext));
                actionQueue.AddRange(updateContext.GetActionList());
                // Currently do not end immediately so we can wait for any late effects triggering
                // if (CheckForEndCondition()) return;
            }
        }

        // Execute and await all actions
        Friendlies.ForEach(participant => { participant.OnTurnEnd(); });
        Enemies.ForEach(participant => { participant.OnTurnEnd(); });

        if (CheckForEndCondition()) return;

        await Task.Delay(1000);

        Task.Run(DoRound);
    }

    private bool CheckForEndCondition()
    {
        if (Friendlies.All(participant => participant.State != ParticipantState.Alive))
        {
            PlayerLost();
            return true;
        }

        if (Enemies.All(participant => participant.State != ParticipantState.Alive))
        {
            PlayerWon();
            return true;
        }

        return false;
    }

    private void PlayerLost()
    {
        _globalEventManager.EmitPostBattle(false, CreateSnapshot());
        _onLoose.Invoke();
    }

    private void PlayerWon()
    {
        _globalEventManager.EmitPostBattle(true, CreateSnapshot());
        _onWin.Invoke();
    }

    private BattleSnapshot CreateSnapshot()
    {
        return new BattleSnapshot
        {
            Friendlies = Friendlies.Select(participant => participant.CreateSnapshot()).ToList(),
            Enemies = Enemies.Select(participant => participant.CreateSnapshot()).ToList()
        };
    }

    public bool IsFriendly(IBattleParticipant participant)
    {
        return Friendlies.Contains(participant);
    }
}