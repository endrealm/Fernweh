using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaAbilityConfigBuilder
{
    private readonly string _id;
    private LuaTable _data;

    public LuaAbilityConfigBuilder(string id)
    {
        _id = id;
    }

    public LuaAbilityConfigBuilder Data(LuaTable data)
    {
        _data = data;
        return this;
    }

    public AbilityConfig Build()
    {
        return new AbilityConfig(_id, _data);
    }
}