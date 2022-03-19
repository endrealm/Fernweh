using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class BattleManager
{
    private readonly List<IBattleParticipant> _friendlies = new();
    private readonly List<IBattleParticipant> _enemies = new();

    public BattleManager(BattleRegistry registry, BattleConfig config)
    {
    }
}