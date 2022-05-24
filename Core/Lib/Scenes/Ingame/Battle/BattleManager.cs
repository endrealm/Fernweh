using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle.Impl;
using Core.Scenes.Ingame.Modes;
using Core.Scenes.Ingame.Views;
using Core.Utils;

namespace Core.Scenes.Ingame.Battle;

public class BattleManager
{
    private readonly IChatView _chatView;
    private readonly BattleRegistry _registry;
    private readonly IPlayerBattleInput _playerInput;
    private readonly Action _onWin;
    private readonly Action _onLoose;
    private readonly List<IBattleParticipant> _friendlies;
    private readonly List<IBattleParticipant> _enemies;

    public BattleManager(IChatView chatView, BattleRegistry registry, BattleConfig config, IPlayerBattleInput playerInput, Action onWin, Action onLoose)
    {
        _chatView = chatView;
        _registry = registry;
        _playerInput = playerInput;
        _onWin = onWin;
        _onLoose = onLoose;
        var enemyDict = new Dictionary<string, int>();
        _enemies = config.Enemies
            .Select(id =>
            {
                if (enemyDict.TryGetValue(id, out var index))
                {
                    enemyDict[id] = ++index;
                }
                else
                {
                    enemyDict.Add(id, ++index);
                }
                
                return CreateParticipant(id+ " " + index, registry.GetParticipantFactory(id).Produce());
            })
            .ToList();
        _friendlies = _registry.FriendlyParticipantsProvider.Load().Select(config1 => CreateParticipant(config1.Id, config1)).ToList();
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

    private IBattleParticipant CreateParticipant(string particpantId, ParticipantConfig config)
    {
        
        var participant = new BasicParticipant(particpantId, config.Id, config);
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
            participant.NextAction ??= participant.Strategy.SelectAction(this, participant);
            actions.Add(participant.NextAction);
            participant.NextAction = null;
        });
        _enemies.ForEach(participant =>
        {
            participant.OnNextTurn(out var skip);
            if (skip) return;
            participant.NextAction ??= participant.Strategy.SelectAction(this, participant);
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

            if (action.CausesStateCheck)
            {
                // Check for any changed states
                var updateContext = new ActionContext(_chatView);
                _friendlies.ForEach(participant => participant.UpdateParticipantState(updateContext));
                _enemies.ForEach(participant => participant.UpdateParticipantState(updateContext));
                actionQueue.AddRange(updateContext.GetActionList());
                // Currently do not end immediately so we can wait for any late effects triggering
                // if (CheckForEndCondition()) return;
            }
        }
        
        // Execute and await all actions
        _friendlies.ForEach(participant => { participant.OnTurnEnd(); });
        _enemies.ForEach(participant => { participant.OnTurnEnd(); });
        
        if (CheckForEndCondition()) return;

        await Task.Delay(2000);

        Task.Run(DoRound);
    }

    private bool CheckForEndCondition()
    {
        if (_friendlies.All(participant => participant.State != ParticipantState.Alive))
        {
            PlayerLost();
            return true;
        }

        if (_enemies.All(participant => participant.State != ParticipantState.Alive))
        {
            PlayerWon();
            return true;
        }

        return false;
    }

    private void PlayerLost()
    {
        _onLoose.Invoke();
    } 
    
    private void PlayerWon()
    {
        _onWin.Invoke();
    }

    public bool IsFriendly(IBattleParticipant participant)
    {
        return _friendlies.Contains(participant);
    }
}