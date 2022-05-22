using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaAbility : IAbility
{
    private readonly LuaFunction _onReceiveDamage;
    private readonly LuaFunction _onDealDamage;
    private readonly LuaFunction _onTargetWithSpell;
    private readonly LuaFunction _onTargetedBySpell;
    private readonly LuaFunction _onCalculateStats;
    private readonly LuaFunction _onNextTurn;
    private readonly LuaFunction _onUse;
    private readonly LuaFunction _canUse;
    private readonly LuaFunction _onTurnEnd;
    public string CategoryId { get; }
    public AbilityTargetType TargetType { get; }
    public string Id { get; }
    public int ManaCost { get; }
    public bool AllowDeadTargets { get; }
    public bool AllowLivingTargets { get; }
    public bool HideBlocked { get; }
    public bool Hidden { get; }

    public LuaAbility(
        LuaFunction onReceiveDamage,
        LuaFunction onDealDamage,
        LuaFunction onTargetWithSpell,
        LuaFunction onTargetedBySpell,
        LuaFunction onCalculateStats,
        LuaFunction onNextTurn,
        LuaFunction onUse,
        LuaFunction canUse,
        LuaFunction onTurnEnd,
        string category,
        string id,
        int manaCost,
        AbilityTargetType targetType, 
        bool allowDeadTargets, 
        bool allowLivingTargets,
        bool hideBlocked,
        bool hidden
    ) {
        _onReceiveDamage = onReceiveDamage;
        _onDealDamage = onDealDamage;
        _onTargetWithSpell = onTargetWithSpell;
        _onTargetedBySpell = onTargetedBySpell;
        _onCalculateStats = onCalculateStats;
        _onNextTurn = onNextTurn;
        _onUse = onUse;
        _canUse = canUse;
        _onTurnEnd = onTurnEnd;
        CategoryId = category;
        Id = id;
        ManaCost = manaCost;
        TargetType = targetType;
        AllowDeadTargets = allowDeadTargets;
        AllowLivingTargets = allowLivingTargets;
        HideBlocked = hideBlocked;
        Hidden = hidden;
    }

    public void OnReceiveDamage(DamageReceiveEvent evt)
    {
        _onReceiveDamage?.Call(evt);
    }

    public void OnDealDamage(DamageDealEvent evt)
    {
        _onDealDamage?.Call(evt);
    }

    public void OnTargetWithSpell(SpellTargetEvent evt)
    {
        _onTargetWithSpell?.Call(evt);
    }

    public void OnTargetedBySpell(SpellTargetEvent evt)
    {
        _onTargetedBySpell?.Call(evt);
    }

    public void OnCalculateStats(Stats stats)
    {
        _onCalculateStats?.Call(stats);
    }

    public void OnNextTurn(out bool skip)
    {
        skip = false;
        if (_onNextTurn == null) return;
        var results = _onNextTurn.Call();
        if (results.Length == 0) return;
        skip = (bool)results.First();
    }

    public void OnTurnEnd()
    {
        _onTurnEnd?.Call();
    }

    public void Use(AbilityUseContext context)
    {
        _onUse?.Call(context);
    }

    public bool CanUse(AbilityUseCheckContext context)
    {
        var props = _canUse?.Call(context);
        if (props is { Length: > 0 }) return (bool)props.First();

        return true;
    }

    public IBattleAction ProduceAction(IBattleParticipant participant, List<IBattleParticipant> targets)
    {
        return new AbilityAction(this, participant, targets);
    }
}