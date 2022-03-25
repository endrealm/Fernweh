using Core.Scenes.Ingame.Views;

namespace Core.Scenes.Ingame.Battle;

public class FallbackStrategy: IBattleStrategy
{
    public IBattleAction SelectAction(IBattleParticipant participant)
    {
        return new TestAction(participant);
    }
}