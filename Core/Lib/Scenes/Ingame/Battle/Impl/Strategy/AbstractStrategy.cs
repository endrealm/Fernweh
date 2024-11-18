using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Scenes.Ingame.Battle.Impl.Strategy;

public abstract class AbstractStrategy : IBattleStrategy
{
    public abstract IBattleAction SelectAction(Random random, BattleManager manager, IBattleParticipant participant);

    /// <summary>
    ///     Finds all usable abilities
    /// </summary>
    /// <param name="participant"></param>
    /// <returns></returns>
    protected IEnumerable<IAbility> GetValidAbilities(IBattleParticipant participant)
    {
        return participant.GetAbilities().Where(ability => ability.ManaCost >= participant.Mana);
    }

    protected List<List<IBattleParticipant>> GetTargets(BattleManager manager, IBattleParticipant participant,
        AbilityTargetType type, bool allowDeadTargets = false, bool allowLivingTargets = true)
    {
        var results = manager.IsFriendly(participant)
            ? TargetTypeUtils.GetTargetsByType(manager.Enemies, manager.Friendlies, manager.All, type)
            : TargetTypeUtils.GetTargetsByType(manager.Friendlies, manager.Enemies, manager.All, type);

        return results.Where(targets =>
        {
            // Filter for dead targets if the spell does not allow this
            if (!allowDeadTargets) targets.RemoveAll(target => target.State == ParticipantState.Dead);

            // Filter for living if only dead are allowed
            if (!allowLivingTargets) targets.RemoveAll(target => target.State == ParticipantState.Alive);

            // skip now empty target groupings
            return targets.Count != 0;
        }).ToList();
    }

    protected WeightConfig CalculateAttackWeight(IBattleParticipant participant)
    {
        return new WeightConfig
        {
            Damage = participant.GetStats().Strength
        };
    }
}