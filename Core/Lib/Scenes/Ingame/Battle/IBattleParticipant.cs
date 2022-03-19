using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IBattleParticipant : IBattleEventReceiver
{
    public string ParticipantId { get; }
    public int Health { get; }
    public int Mana { get; }
    public List<IAbility> GetAbilities();
    public List<IStatusEffect> GetActiveEffects();
}