using System;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaEffectFactoryBuilder
{
    private readonly string _id;
    private readonly Action<IEffectFactory> _onBuild;
    private LuaFunction _instantiateEffect;

    public LuaEffectFactoryBuilder(string id, Action<IEffectFactory> onBuild)
    {
        _id = id;
        _onBuild = onBuild;
    }

    public LuaEffectFactoryBuilder Instantiate(LuaFunction function)
    {
        _instantiateEffect = function;
        return this;
    }

    public IEffectFactory Build()
    {
        var fact = new LuaEffectFactory(_id, _instantiateEffect);
        _onBuild.Invoke(fact);
        return fact;
    }
}