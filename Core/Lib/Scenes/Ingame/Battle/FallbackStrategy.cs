using Core.Scenes.Ingame.Battle.Impl.Actions;

namespace Core.Scenes.Ingame.Battle;

public class FallbackStrategy: IBattleStrategy
{
    public IBattleAction SelectAction(IBattleParticipant participant)
    {
        return new DoNothingAction(participant);
    }
}