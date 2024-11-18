using System.Threading.Tasks;

namespace Core.Scenes.Ingame.Battle;

public interface IBattleAction
{
    IBattleParticipant Participant { get; }
    int Priority { get; }
    bool AllowDeath { get; }
    bool CausesStateCheck { get; }
    Task DoAction(ActionContext context);
}