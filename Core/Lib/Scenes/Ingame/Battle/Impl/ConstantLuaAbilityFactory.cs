namespace Core.Scenes.Ingame.Battle.Impl;

public class ConstantLuaAbilityFactory : IAbilityFactory
{
    private readonly LuaAbility _ability;

    public ConstantLuaAbilityFactory(string id, LuaAbility ability)
    {
        _ability = ability;
        Id = id;
    }

    public string Id { get; }

    public IAbility Produce(AbilityConfig config)
    {
        return _ability;
    }
}