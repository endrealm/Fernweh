using NLua;

namespace Core.Scenes.Ingame.Battle;

public struct WeightConfig
{
    public WeightConfig()
    {
    }

    public int Damage { get; set; } = 0;
    public int Heal { get; set; } = 0;
    public int Revive { get; set; } = 0;
    public int Buff { get; set; } = 0;
    public int Debuff { get; set; } = 0;

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