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
}

public struct SpellTargetEvent
{
    public IBattleParticipant Target;
    public IBattleParticipant Source;
    public bool PreventReflect;
    public SpellData Data;
}

public struct DamageDealEvent
{
    public IBattleParticipant Target;
    public IBattleParticipant Source;
    public DamageData Data;
}