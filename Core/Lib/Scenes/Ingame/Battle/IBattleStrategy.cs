using System;

namespace Core.Scenes.Ingame.Battle;

public interface IBattleStrategy
{
    public IBattleAction SelectAction(Random random, BattleManager manager, IBattleParticipant participant);
}