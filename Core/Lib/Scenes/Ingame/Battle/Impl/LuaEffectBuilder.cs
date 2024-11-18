using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaEffectBuilder
{
    private LuaFunction _onCalculateStats;
    private LuaFunction _onDealDamage;
    private LuaFunction _onNextTurn;
    private LuaFunction _onReceiveDamage;
    private LuaFunction _onTargetedBySpell;
    private LuaFunction _onTargetWithSpell;
    private LuaFunction _onTryCleanse;
    private LuaFunction _onTurnEnd;

    public LuaEffectBuilder OnReceiveDamage(LuaFunction function)
    {
        _onReceiveDamage = function;
        return this;
    }

    public LuaEffectBuilder OnDealDamage(LuaFunction function)
    {
        _onDealDamage = function;
        return this;
    }

    public LuaEffectBuilder OnTargetWithSpell(LuaFunction function)
    {
        _onTargetWithSpell = function;
        return this;
    }

    public LuaEffectBuilder OnTargetedBySpell(LuaFunction function)
    {
        _onTargetedBySpell = function;
        return this;
    }

    public LuaEffectBuilder OnCalculateStats(LuaFunction function)
    {
        _onCalculateStats = function;
        return this;
    }

    public LuaEffectBuilder OnNextTurn(LuaFunction function)
    {
        _onNextTurn = function;
        return this;
    }

    public LuaEffectBuilder OnTryCleanse(LuaFunction function)
    {
        _onTryCleanse = function;
        return this;
    }

    public LuaEffectBuilder OnTurnEnd(LuaFunction function)
    {
        _onTurnEnd = function;
        return this;
    }

    public IStatusEffect Build()
    {
        return new LuaStatusEffect(_onReceiveDamage, _onDealDamage, _onTargetWithSpell, _onTargetedBySpell,
            _onCalculateStats, _onNextTurn, _onTryCleanse, _onTurnEnd);
    }
}