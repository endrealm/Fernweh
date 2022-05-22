using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IAbility : IBattleEventReceiver
{
    void Use(AbilityUseContext context);
    bool CanUse(AbilityUseCheckContext context);
    WeightConfig CalculateWeight(AbilityWeightContext context);
    string CategoryId { get; }
    AbilityTargetType TargetType { get; }
    string Id { get; }
    int ManaCost { get; }
    bool AllowDeadTargets { get; }
    bool AllowLivingTargets { get; }
    public bool HideBlocked { get; }
    public bool Hidden { get; }
    IBattleAction ProduceAction(IBattleParticipant participant, List<IBattleParticipant> targets);
}