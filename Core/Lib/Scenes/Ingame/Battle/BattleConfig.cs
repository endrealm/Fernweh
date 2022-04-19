using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class BattleConfig
{
    public BattleConfig(List<string> enemies)
    {
        Enemies = enemies;
    }

    public List<string> Enemies { get; }
}