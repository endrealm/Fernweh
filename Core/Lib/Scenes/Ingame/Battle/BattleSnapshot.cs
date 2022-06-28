using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public struct BattleSnapshot
{
    public List<ParticipantSnapshot> Friendlies { get; set; }
    public List<ParticipantSnapshot> Enemies { get; set; }
}

public struct ParticipantSnapshot
{
    public ParticipantConfig Config { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Experience { get; set; }
}