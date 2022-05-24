using Core.Scenes.Ingame.Battle.Impl.Actions;

namespace Core.Scenes.Ingame.Battle.Impl.Strategy;

public class FallbackStrategy: IBattleStrategy
{
    public IBattleAction SelectAction(BattleManager manager, IBattleParticipant participant)
    {
        return new DoNothingAction(participant);
    }
}