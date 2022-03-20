using System.Collections.Generic;
using Core.Utils;

namespace Core.Scenes.Ingame.Battle;

public class BattleManager
{
    private readonly IPlayerBattleInput _playerInput;
    private readonly List<IBattleParticipant> _friendlies = new();
    private readonly List<IBattleParticipant> _enemies = new();

    public BattleManager(BattleRegistry registry, BattleConfig config, IPlayerBattleInput playerInput)
    {
        // TODO: generate enemies from registry using the config
        _playerInput = playerInput;
    }

    public async void DoRound()
    {
        // Assigns actions to all player controlled units
        await _playerInput.HandlePlayerInput();
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