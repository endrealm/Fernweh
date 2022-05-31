using System.Collections.Generic;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle;

public interface IConsumable
{
    public int Amount { get; }
    public ChatWrapper Name { get; }
    public IAbility Ability { get; }

    IBattleAction ProduceAction(IBattleParticipant participant, List<IBattleParticipant> targets);
}