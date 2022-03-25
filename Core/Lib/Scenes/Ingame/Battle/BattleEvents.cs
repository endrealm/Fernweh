using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

// Class uses lowercase names for LUA script exposal

public struct DamageData
{
    public int Damage;
    public Element Element;
    // add more data
}

public struct SpellData
{
    //add more data ofc
}


public struct DamageReceiveEvent
{
    public IBattleParticipant Target;
    public IBattleParticipant Source;
    public DamageData Data;

    public DamageReceiveEvent(IBattleParticipant target, IBattleParticipant source, DamageData data)
    {
        Target = target;
        Source = source;
        Data = data;
    }
}

public class SpellTargetEvent
{
    public List<IBattleParticipant> Targets;
    public IBattleParticipant Source;
    public bool PreventReflect;
    public SpellData Data;

    public SpellTargetEvent(List<IBattleParticipant> targets, IBattleParticipant source, bool preventReflect,
        SpellData data)
    {
        Targets = targets;
        Source = source;
        PreventReflect = preventReflect;
        Data = data;
    }
}

public struct DamageDealEvent
{
    public IBattleParticipant Target;
    public IBattleParticipant Source;
    public DamageData Data;

    public DamageDealEvent(IBattleParticipant target, IBattleParticipant source, DamageData data)
    {
        Target = target;
        Source = source;
        Data = data;
    }
}