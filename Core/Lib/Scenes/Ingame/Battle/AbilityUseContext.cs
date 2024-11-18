using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class AbilityUseContext
{
    private readonly ActionContext _actionContext;

    public AbilityUseContext(ActionContext actionContext, IBattleParticipant participant,
        List<IBattleParticipant> targets)
    {
        _actionContext = actionContext;
        Participant = participant;
        Targets = targets;
    }

    public IBattleParticipant Participant { get; }
    public List<IBattleParticipant> Targets { get; }

    public void QueueAction(IBattleAction battleAction)
    {
        _actionContext.QueueAction(battleAction);
    }
}

public class AbilityUseCheckContext
{
    public AbilityUseCheckContext(IBattleParticipant participant)
    {
        Participant = participant;
    }

    public IBattleParticipant Participant { get; }
}

public class AbilityWeightContext
{
    public AbilityWeightContext(IBattleParticipant participant)
    {
        Participant = participant;
    }

    public IBattleParticipant Participant { get; }
}