﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class DealDamageAction : IBattleAction
{
    private readonly DamageData _data;
    private readonly List<IBattleParticipant> _targets;

    public DealDamageAction(DamageData data, IBattleParticipant participant, List<IBattleParticipant> targets)
    {
        _data = data;
        Participant = participant;
        _targets = targets;
    }

    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        _targets.ForEach(target =>
        {
            var data = _data.Clone();
            data.Damage -= target.GetStats().Constitution / 5 + target.GetStats().Armor;
            data.Damage = Math.Max(data.Damage, 0);
            Participant.OnDealDamage(new DamageDealEvent(target, Participant, data));
            target.OnReceiveDamage(new DamageReceiveEvent(target, Participant, data));
            context.QueueAction(new LogTextAction("battle.dealDamage",
                new TextReplacement("target", target.DisplayName),
                new TextReplacement("damage", data.Damage.ToString())
            ));
        });
        context.QueueAction(new AwaitNextAction());
    }

    public int Priority => -1;
    public bool AllowDeath { get; } = true;
    public bool CausesStateCheck { get; } = false;
}