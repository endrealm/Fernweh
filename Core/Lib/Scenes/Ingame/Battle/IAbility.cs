using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IAbility : IBattleEventReceiver
{
    void Use(AbilityUseContext context);
    bool CanUse(AbilityUseCheckContext context);
    string CategoryId { get; }
    string Id { get; }
    int ManaCost { get; }
    IBattleAction ProduceAction(IBattleParticipant participant, List<IBattleParticipant> targets);
}