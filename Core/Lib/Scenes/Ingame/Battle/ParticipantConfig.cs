using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public readonly record struct ParticipantConfig(string Id,
    Stats Stats,
    List<AbilityConfig> Abilities,
    int Health = -1,
    int Mana = -1
)
{
    public string Id { get; } = Id;
    public int Health { get; } = Health;
    public int Mana { get; } = Mana;
    public Stats Stats { get; } = Stats;

    public List<AbilityConfig> Abilities { get; } = Abilities;
}