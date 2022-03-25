using System.Linq;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaAbilityFactory : IAbilityFactory
{
    private readonly LuaFunction _produce;
    public string Id { get; }

    public LuaAbilityFactory(string id, LuaFunction produce)
    {
        Id = id;
        _produce = produce;
    }

    public IAbility Produce(AbilityConfig config)
    {
        return (IAbility)_produce.Call(new LuaAbilityBuilder(Id), config).First();
    }
}