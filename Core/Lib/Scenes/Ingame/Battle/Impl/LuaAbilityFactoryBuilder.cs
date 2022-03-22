using System;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaAbilityFactoryBuilder
{
    private LuaFunction _instantiateAbility;
    private readonly string _id;
    private readonly Action<IAbilityFactory> _onBuild;

    public LuaAbilityFactoryBuilder(string id, Action<IAbilityFactory> onBuild)
    {
        _id = id;
        _onBuild = onBuild;
    }

    public LuaAbilityFactoryBuilder Instantiate(LuaFunction function)
    {
        _instantiateAbility = function;
        return this;
    }

    public IAbilityFactory Build()
    {
        var fact = new LuaAbilityFactory(_id, _instantiateAbility);
        _onBuild.Invoke(fact);
        return fact;
    }
}