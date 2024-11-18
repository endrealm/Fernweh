using System;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaParticipantFactoryBuilder
{
    private readonly string _id;
    private readonly Action<IParticipantFactory> _onBuild;
    private LuaFunction _instantiateEffect;

    public LuaParticipantFactoryBuilder(string id, Action<IParticipantFactory> onBuild)
    {
        _id = id;
        _onBuild = onBuild;
    }

    public LuaParticipantFactoryBuilder Instantiate(LuaFunction function)
    {
        _instantiateEffect = function;
        return this;
    }

    public IParticipantFactory Build()
    {
        var fact = new LuaParticipantFactory(_id, _instantiateEffect);
        _onBuild.Invoke(fact);
        return fact;
    }
}