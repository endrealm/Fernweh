using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Chat;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions
{
    internal class HealManaAction : IBattleAction
    {
        public IBattleParticipant Participant { get; }
        private List<IBattleParticipant> _targets;
        private int _amount;

        public HealManaAction(IBattleParticipant participant, List<IBattleParticipant> targets, int amount)
        {
            Participant = participant;
            _targets = targets;
            _amount = amount;
        }

        public async Task DoAction(ActionContext context)
        {
            _targets.ForEach(target =>
            {
                int manaAdded = target.HealMana(_amount);
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
}
