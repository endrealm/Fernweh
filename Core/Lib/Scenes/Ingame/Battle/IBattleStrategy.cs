namespace Core.Scenes.Ingame.Battle;

public interface IBattleStrategy
{
    public IBattleAction SelectAction(IBattleParticipant participant);
}