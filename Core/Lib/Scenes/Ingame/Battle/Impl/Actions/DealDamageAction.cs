using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class DealDamageAction: IBattleAction
{
    public IBattleParticipant Participant { get; }
    private List<IBattleParticipant> _targets;
    private DamageData _data;
    public DealDamageAction(DamageData data, IBattleParticipant participant, List<IBattleParticipant> targets)
    {
        _data = data;
        Participant = participant;
        _targets = targets;
    }

    public async Task DoAction(ActionContext context)
    {
        _targets.ForEach(target =>
        {
            var data = _data.Clone();
            Participant.OnDealDamage(new DamageDealEvent(target, Participant, data));
            target.OnReceiveDamage(new DamageReceiveEvent(target, Participant, data));
            context.QueueAction(new LogTextAction("battle.dealDamage", 
                new Replacement("target", target.ParticipantId),
                    new Replacement("damage", data.Damage.ToString())
                ));
            context.QueueAction(new AwaitNextAction());
        });
    }

    public int Priority => -1;
    public bool AllowDeath { get; set; } = true;
}