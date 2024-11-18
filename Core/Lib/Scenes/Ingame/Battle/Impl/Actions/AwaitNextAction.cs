using System.Threading.Tasks;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class AwaitNextAction : IBattleAction
{
    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        var done = false;
        context.AddAction("battle.continue", () => done = true);
        while (!done) await Task.Delay(50);
        context.ClearChat();
    }

    public int Priority => -1;
    public bool AllowDeath { get; } = true;
    public bool CausesStateCheck { get; } = true;
}