using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class AbilityUseContext
{
    public IBattleParticipant Participant { get; }
    public List<IBattleParticipant> Targets { get; }

    public AbilityUseContext(IBattleParticipant participant, List<IBattleParticipant> targets)
    {
        Participant = participant;
        Targets = targets;
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