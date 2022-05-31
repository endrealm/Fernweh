using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Chat;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class AttackAction: IBattleAction
{
    private readonly IBattleParticipant _target;
    public IBattleParticipant Participant { get; }
    public AttackAction(IBattleParticipant participant, IBattleParticipant target)
    {
        _target = target;
        Participant = participant;
    }

    public async Task DoAction(ActionContext context)
    {
        context.QueueAction(new LogTextAction("battle.action.attack", new TextReplacement("name", Participant.DisplayName), new TextReplacement("target", _target.DisplayName)));
        // Filter for dead targets if the spell does not allow this
        if (_target.State != ParticipantState.Alive)
        {
            context.QueueAction(new LogTextAction("ability.noTargets"));
            context.QueueAction(new AwaitNextAction());
            return;
        }
        //context.QueueAction(new AwaitNextAction());
        var data = new DamageData(Participant.GetStats().Strength, Element.None);
        context.QueueAction(new DealDamageAction(data, Participant, new List<IBattleParticipant>() {_target}));
        context.SoundPlayer.PlaySFX("damage_small");
    }

    public int Priority => Participant.GetStats().Dexterity; // Defends trigger at beginning
    public bool AllowDeath { get; } = false;
    public bool CausesStateCheck { get; } = false;
}