using System.Collections.Generic;
using System.Threading.Tasks;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

internal class HealManaAction : IBattleAction
{
    private readonly int _amount;
    private readonly List<IBattleParticipant> _targets;

    public HealManaAction(IBattleParticipant participant, List<IBattleParticipant> targets, int amount)
    {
        Participant = participant;
        _targets = targets;
        _amount = amount;
    }

    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        _targets.ForEach(target =>
        {
            var manaAdded = target.HealMana(_amount);
            context.QueueAction(new LogTextAction("battle.action.healMana",
                new TextReplacement("amount", manaAdded.ToString()),
                new TextReplacement("name", Participant.DisplayName),
                new TextReplacement("target", target.DisplayName)));
            context.QueueAction(new AwaitNextAction());
        });
    }

    public int Priority => Participant.GetStats().Dexterity; // Defends trigger at beginning
    public bool AllowDeath { get; } = false;
    public bool CausesStateCheck { get; } = false;
}