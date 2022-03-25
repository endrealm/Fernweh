using System.Threading.Tasks;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class AwaitNextAction: IBattleAction
{

    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        var done = false;
        context.AddAction("continue", () => done = true);
        while (!done) await Task.Delay(50);
        context.ClearChat();
    }

    public int Priority => -1;
}