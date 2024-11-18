using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class ConsumableUseAction : IBattleAction
{
    private readonly IConsumable _consumable;
    private readonly Action _onUse;
    private readonly List<IBattleParticipant> _targets;

    public ConsumableUseAction(IBattleParticipant participant, IConsumable consumable, List<IBattleParticipant> targets,
        Action onUse)
    {
        Participant = participant;
        _consumable = consumable;
        _targets = targets;
        _onUse = onUse;
    }

    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        //context.QueueAction(new LogTextAction("battle.use.item", 
        //    new TextReplacement("player", Participant.DisplayName),
        //    new WrapperReplacement("item", _consumable.Name)
        //));
        _onUse.Invoke();
        var action = _consumable.Ability.ProduceAction(Participant, _targets);
        context.QueueAction(action); // TODO: add option to force reflect prevention
    }

    public int Priority => 999999; // => trigger at start but after defend
    public bool AllowDeath => false;
    public bool CausesStateCheck => false;
}