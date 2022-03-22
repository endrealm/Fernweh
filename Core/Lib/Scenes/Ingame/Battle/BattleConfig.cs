using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class BattleConfig
{
    public BattleConfig(List<string> enemies, List<ParticipantConfig> friendlies)
    {
        Enemies = enemies;
        Friendlies = friendlies;
    }

    public List<string> Enemies { get; }
    public List<ParticipantConfig> Friendlies { get; }
}