using System.Collections.Generic;
using System.Linq;

namespace Core.Scenes.Ingame.Battle.Impl.Strategy;

public abstract class AbstractStrategy: IBattleStrategy
{
    public abstract IBattleAction SelectAction(BattleManager manager, IBattleParticipant participant);

    /// <summary>
    /// Finds all usable abilities
    /// </summary>
    /// <param name="participant"></param>
    /// <returns></returns>
    protected IEnumerable<IAbility> GetValidAbilities(IBattleParticipant participant)
    {
        return participant.GetAbilities().Where(ability => ability.ManaCost >= participant.Mana);
    }

    protected List<List<IBattleParticipant>> GetTargets(BattleManager manager, IBattleParticipant participant, AbilityTargetType type)
    {
        return manager.IsFriendly(participant) ? 
            TargetTypeUtils.GetTargetsByType(manager.Enemies, manager.Friendlies, manager.All, type) 
            : TargetTypeUtils.GetTargetsByType(manager.Friendlies, manager.Enemies, manager.All, type);
    }

    protected WeightConfig CalculateAttackWeight(IBattleParticipant participant)
    {
        return new WeightConfig
        {
            Damage = participant.GetStats().Strength
        };
    }
}