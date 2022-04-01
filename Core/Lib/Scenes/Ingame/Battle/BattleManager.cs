using System.Collections.Generic;
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
        _friendlies = config.Friendlies.Select(config1 => CreateParticipant(config1.Id, config1)).ToList();
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

            if (action.CausesStateCheck)
            {
                // Check for any changed states
                var updateContext = new ActionContext(_chatView);
                _friendlies.ForEach(participant => participant.UpdateParticipantState(updateContext));
                _enemies.ForEach(participant => participant.UpdateParticipantState(updateContext));
                actionQueue.AddRange(updateContext.GetActionList());
            }
        }
        
        // Execute and await all actions
        _friendlies.ForEach(participant => { participant.OnTurnEnd(); });
        _enemies.ForEach(participant => { participant.OnTurnEnd(); });
        await Task.Delay(2000);

        Task.Run(DoRound);
    }
    
}