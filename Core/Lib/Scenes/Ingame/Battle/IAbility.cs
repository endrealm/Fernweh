using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IAbility : IBattleEventReceiver
{
    string CategoryId { get; }
    AbilityTargetType TargetType { get; }
    string Id { get; }
    int ManaCost { get; }
    bool AllowDeadTargets { get; }
    bool AllowLivingTargets { get; }
    public bool HideBlocked { get; }
    public bool Hidden { get; }
    void Use(AbilityUseContext context);
    bool CanUse(AbilityUseCheckContext context);
    WeightConfig CalculateWeight(AbilityWeightContext context);
    IBattleAction ProduceAction(IBattleParticipant participant, List<IBattleParticipant> targets);
}