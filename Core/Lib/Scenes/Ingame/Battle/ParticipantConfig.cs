using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public readonly record struct ParticipantConfig(string Id,
    Stats Stats,
    List<AbilityConfig> Abilities
)
{
    public string Id { get; } = Id;
    public Stats Stats { get; } = Stats;

    public List<AbilityConfig> Abilities { get; } = Abilities;
}