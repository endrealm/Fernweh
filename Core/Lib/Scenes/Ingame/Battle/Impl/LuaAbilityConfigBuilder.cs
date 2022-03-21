namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaAbilityConfigBuilder
{
    private string _id;

    public LuaAbilityConfigBuilder(string id)
    {
        _id = id;
    }

    public AbilityConfig Build()
    {
        return new AbilityConfig(_id);
    }
}