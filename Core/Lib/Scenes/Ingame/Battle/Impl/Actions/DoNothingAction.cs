using System.Threading.Tasks;
using Core.Scenes.Ingame.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class DoNothingAction: IBattleAction
{
    public DoNothingAction(IBattleParticipant participant)
    {
        Priority = participant.GetStats().Agility;
        Participant = participant;
    }

    public IBattleParticipant Participant { get; }
    public async Task DoAction(ActionContext context)
    {
        context.QueueAction(new LogTextAction("ability.doNothing", new Replacement("name", Participant.DisplayName)));
        context.QueueAction(new AwaitNextAction());
    }

    public int Priority { get; }
    public bool AllowDeath { get; set; }
    public bool CausesStateCheck { get; } = false;

}