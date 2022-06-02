using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class ConsumableUseAction: IBattleAction
{
    public IBattleParticipant Participant { get; }

    private readonly IConsumable _consumable;
    private readonly List<IBattleParticipant> _targets;
    private readonly Action _onUse;

    public ConsumableUseAction(IBattleParticipant participant, IConsumable consumable, List<IBattleParticipant> targets, Action onUse)
    {
        Participant = participant;
        _consumable = consumable;
        _targets = targets;
        _onUse = onUse;
    }

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