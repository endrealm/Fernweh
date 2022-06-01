using System.Collections.Generic;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using NLua;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaConsumable: IConsumable
{
    private readonly LuaFunction _onUse;

    public LuaConsumable(int amount, ChatWrapper name, IAbility ability,  LuaFunction onUse)
    {
        _onUse = onUse;
        Amount = amount;
        Name = name;
        Ability = ability;
    }

    public int Amount { get; private set; }
    public ChatWrapper Name { get; }
    public IAbility Ability { get; }
    public IBattleAction ProduceAction(IBattleParticipant participant, List<IBattleParticipant> targets)
    {
        Amount--;
        return new ConsumableUseAction(participant, this, targets, () =>
        {
            _onUse.Call();
        });
    }
}