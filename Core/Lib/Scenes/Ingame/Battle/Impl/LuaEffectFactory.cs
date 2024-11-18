using System.Linq;
using Core.Utils;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaEffectFactory : IEffectFactory
{
    private readonly LuaFunction _effectCreateFunction;

    public LuaEffectFactory(string effectId, LuaFunction effectCreateFunction)
    {
        _effectCreateFunction = effectCreateFunction;
        EffectId = effectId;
    }

    public string EffectId { get; }

    public IStatusEffect Produce(IBattleParticipant target, PropsArray props)
    {
        return (IStatusEffect) _effectCreateFunction.Call(new LuaEffectBuilder(), target, props).First();
    }
}