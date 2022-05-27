using System;
using System.Linq;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using Core.Utils;

namespace Core.Scenes.Ingame.Battle.Impl.Strategy;

public class AggressiveStrategy: AbstractStrategy
{
    public override IBattleAction SelectAction(Random random, BattleManager manager, IBattleParticipant participant)
    {
        var highestAbility = GetValidAbilities(participant)
            .SelectMany(ability =>
            {
                return GetTargets(manager, participant, ability.TargetType, ability.AllowDeadTargets, ability.AllowLivingTargets)
                    .Select(targets =>
                    {
                        return new WrappedAction(() => ability.ProduceAction(participant, targets),
                            ability.CalculateWeight(new AbilityWeightContext(participant)));
                    });
            });
        var attackWeight = CalculateAttackWeight(participant);
        GetTargets(manager, participant, AbilityTargetType.EnemySingle)
            .ForEach(targets =>
            {
                highestAbility = highestAbility.Append(new WrappedAction(() => new AttackAction(participant, targets.First()),
                    attackWeight));
            });
        return highestAbility.MaxValues(action => action.Config.Damage).Random(random).Action.Invoke();
    }
}

public class WrappedAction
{
    public Func<IBattleAction> Action { get; }
    public WeightConfig Config { get; }

    public WrappedAction(Func<IBattleAction> action, WeightConfig config)
    {
        Action = action;
        Config = config;
    }
}