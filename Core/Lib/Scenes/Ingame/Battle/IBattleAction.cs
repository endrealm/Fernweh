using System.Threading.Tasks;

namespace Core.Scenes.Ingame.Battle;

public interface IBattleAction
{
    IBattleParticipant Participant { get; }
    Task DoAction(ActionContext context);
    int Priority { get; }
    bool AllowDeath { get; }
    bool CausesStateCheck { get; }
}