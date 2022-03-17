
using System.Linq;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaStatusEffect: IStatusEffect
{
    private readonly LuaFunction _onReceiveDamage;
    private readonly LuaFunction _onDealDamage;
    private readonly LuaFunction _onTargetWithSpell;
    private readonly LuaFunction _onTargetedBySpell;
    private readonly LuaFunction _onCalculateStats;
    private readonly LuaFunction _onNextTurn;
    private readonly LuaFunction _onTryCleanse;

    public LuaStatusEffect(LuaFunction onReceiveDamage, LuaFunction onDealDamage, LuaFunction onTargetWithSpell, LuaFunction onTargetedBySpell, LuaFunction onCalculateStats, LuaFunction onNextTurn, LuaFunction onTryCleanse)
    {
        _onReceiveDamage = onReceiveDamage;
        _onDealDamage = onDealDamage;
        _onTargetWithSpell = onTargetWithSpell;
        _onTargetedBySpell = onTargetedBySpell;
        _onCalculateStats = onCalculateStats;
        _onNextTurn = onNextTurn;
        _onTryCleanse = onTryCleanse;
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
        if(_onNextTurn == null) return;
        var results = _onNextTurn.Call();
        if(results.Length == 0) return;
        skip = (bool)results.First();
    }

    public void OnTryCleanse(out bool persist)
    {
        persist = false;
        if(_onTryCleanse == null) return;
        var results = _onTryCleanse.Call();
        if(results.Length == 0) return;
        persist = (bool)results.First();
        
    }
}