using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IBattleParticipant : IBattleEventReceiver
{
    public string ParticipantId { get; }
    public int Health { get; }
    public ParticipantState State { get; }
    public int Mana { get; }
    public IBattleStrategy Strategy { get; set; }
    public List<IAbility> GetAbilities();
    public List<IStatusEffect> GetActiveEffects();
    IBattleAction NextAction { get; set; }
    Stats GetStats();
    public void UpdateParticipantState();
}