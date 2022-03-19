namespace Core.Scenes.Ingame.Battle;

public interface IBattleAction
{
    IBattleParticipant Participant { get; }
    void DoAction(ActionContext context);
    int Priority { get; }
}