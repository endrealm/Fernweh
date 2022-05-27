using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Chat;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class AbilityAction : IBattleAction
{
    private readonly IAbility _ability;
    private readonly List<IBattleParticipant> _targets;

    public AbilityAction(IAbility ability, IBattleParticipant participant, List<IBattleParticipant> targets)
    {
        _ability = ability;
        _targets = targets;
        Participant = participant;
    }

    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        // Filter for dead targets if the spell does not allow this
        if (!_ability.AllowDeadTargets)
        {
            _targets.RemoveAll(target => target.State == ParticipantState.Dead);
        }
        // Filter for living if only dead are allowed
        if (!_ability.AllowLivingTargets)
        {
            _targets.RemoveAll(target => target.State == ParticipantState.Alive);
        }

        if (Participant.Mana < _ability.ManaCost)
        {
            context.QueueAction(new LogTextAction("ability.noMana", new TextReplacement("name", Participant.DisplayName)));
            context.QueueAction(new AwaitNextAction());
            return;
        }
        
        var spellEvent = new SpellTargetEvent(_targets, Participant, false, new SpellData(_ability.ManaCost));
        Participant.OnTargetWithSpell(spellEvent);
        _targets.ForEach(target => target.OnTargetedBySpell(spellEvent));
        context.QueueAction(
            new LogTextAction("ability.used",
                new TextReplacement("ability", _ability.Id),
                new TextReplacement("caster", Participant.DisplayName)
            )
        );
        
        if (spellEvent.Targets.Count == 0)
        {
            context.QueueAction(new LogTextAction("ability.noTargets"));
            context.QueueAction(new AwaitNextAction());
            return;
        }

        //Participant.DeductMana(_ability.ManaCost); // Calling this will use double the mana?

        // Spell has been reflected
        if (spellEvent.Source != Participant) context.QueueAction(new LogTextAction("ability.reflected"));
        context.QueueAction(new AwaitNextAction());
        _ability.Use(new AbilityUseContext(context, spellEvent.Source, spellEvent.Targets));
    }

    public int Priority => Participant.GetStats().Dexterity;
    public bool AllowDeath { get; }
    public bool CausesStateCheck { get; } = true;
}