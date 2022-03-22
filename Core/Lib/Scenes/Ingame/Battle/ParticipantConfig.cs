using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public readonly record struct ParticipantConfig(string Id,
    int Health,
    int Mana,
    Stats Stats,
    List<AbilityConfig> Abilities
)
{
    public string Id { get; } = Id;
    public int Health { get; } = Health;
    public int Mana { get; } = Mana;
    public Stats Stats { get; } = Stats;

    public List<AbilityConfig> Abilities { get; } = Abilities;
}