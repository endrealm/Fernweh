using NLua;

namespace Core.Scenes.Ingame.Battle;

public struct WeightConfig
{
    public WeightConfig()
    {
    }

    public int Damage { get; private set; } = 0;
    public int Heal { get; private set; } = 0;
    public int Revive { get; private set; } = 0;
    public int Buff { get; private set; } = 0;
    public int Debuff { get; private set; } = 0;

    public static WeightConfig Produce(LuaTable value)
    {
        return new WeightConfig
        {
            Damage = (int) value["damage"],
            Heal = (int) value["heal"],
            Revive = (int) value["revive"],
            Buff = (int) value["buff"],
            Debuff = (int) value["debuff"],
        };
    }
}