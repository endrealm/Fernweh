using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IBattleParticipant : IBattleEventReceiver
{
    /// <summary>
    /// A mostly random battle unique id
    /// </summary>
    public string ParticipantId { get; }
    /// <summary>
    /// The id of the participant type this is part of. It is the same ID used
    /// when creating new enemies
    /// </summary>
    public string GroupId { get; }
    /// <summary>
    /// The name displayed in the UI
    /// </summary>
    public string DisplayName { get; }
    public int Health { get; }
    public ParticipantState State { get; }
    public int Mana { get; }
    public bool Defending { get; set; }
    public IBattleStrategy Strategy { get; set; }
    public List<IAbility> GetAbilities();
    public List<IStatusEffect> GetActiveEffects();
    IBattleAction NextAction { get; set; }
    Stats GetStats();
    /// <summary>
    /// Checks for state updates on the participant. Supplied context should
    /// and will only be used to log state changes.
    /// </summary>
    /// <param name="updateContext"></param>
    public void UpdateParticipantState(ActionContext updateContext);
}