using System.Threading.Tasks;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class DefendAction : IBattleAction
{
    public DefendAction(IBattleParticipant participant)
    {
        Participant = participant;
    }

    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        Participant.Defending = true;
        context.QueueAction(new LogTextAction("battle.action.defend",
            new TextReplacement("name", Participant.DisplayName)));
        context.QueueAction(new AwaitNextAction());
    }

    public int Priority => -1; // Defends trigger at beginning
    public bool AllowDeath { get; } = false;
    public bool CausesStateCheck { get; } = false;
}