using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

// Class uses lowercase names for LUA script exposal

public class DamageData
{
    public DamageData(int damage, Element element)
    {
        Damage = damage;
        Element = element;
    }

    public int Damage { get; set; }
    public Element Element { get; }

    public DamageData Clone()
    {
        return new DamageData(Damage, Element);
    }
}

public class SpellData
{
    public SpellData(int manaCost)
    {
        ManaCost = manaCost;
    }

    public int ManaCost { get; }
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
    public SpellData Data;
    public bool PreventReflect;
    public IBattleParticipant Source;
    public List<IBattleParticipant> Targets;

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