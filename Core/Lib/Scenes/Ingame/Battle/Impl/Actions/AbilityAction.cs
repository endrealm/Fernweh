﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Chat;

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
        var spellEvent = new SpellTargetEvent(_targets, Participant, false, new SpellData(_ability.ManaCost));
        Participant.OnTargetWithSpell(spellEvent);
        _targets.ForEach(target => target.OnTargetedBySpell(spellEvent));
        context.QueueAction(
            new LogTextAction("ability.used",
                new Replacement("ability", _ability.Id),
                new Replacement("caster", Participant.DisplayName)
            )
        );
        // Spell has been reflected
        if (spellEvent.Source != Participant) context.QueueAction(new LogTextAction("ability.reflected"));
        context.QueueAction(new AwaitNextAction());
        _ability.Use(new AbilityUseContext(context, spellEvent.Source, spellEvent.Targets));
    }

    public int Priority => Participant.GetStats().Agility;
    public bool AllowDeath { get; set; }
}