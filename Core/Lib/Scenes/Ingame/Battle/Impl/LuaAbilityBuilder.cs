using System;
using JetBrains.Annotations;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaAbilityBuilder
{
    [CanBeNull] private readonly Action<LuaAbility> _onAbilityBuild;
    private LuaFunction _onReceiveDamage;
    private LuaFunction _onDealDamage;
    private LuaFunction _onTargetWithSpell;
    private LuaFunction _onTargetedBySpell;
    private LuaFunction _onCalculateStats;
    private LuaFunction _onNextTurn;
    private LuaFunction _onTurnEnd;
    private LuaFunction _onUse;
    private LuaFunction _canUse;

    public LuaAbilityBuilder(Action<LuaAbility> onAbilityBuild = null)
    {
        _onAbilityBuild = onAbilityBuild;
    }

    public LuaAbilityBuilder OnReceiveDamage(LuaFunction function)
    {
        _onReceiveDamage = function;
        return this;
    }

    public LuaAbilityBuilder OnDealDamage(LuaFunction function)
    {
        _onDealDamage = function;
        return this;
    }

    public LuaAbilityBuilder OnTargetWithSpell(LuaFunction function)
    {
        _onTargetWithSpell = function;
        return this;
    }

    public LuaAbilityBuilder OnTargetedBySpell(LuaFunction function)
    {
        _onTargetedBySpell = function;
        return this;
    }

    public LuaAbilityBuilder OnCalculateStats(LuaFunction function)
    {
        _onCalculateStats = function;
        return this;
    }

    public LuaAbilityBuilder OnNextTurn(LuaFunction function)
    {
        _onNextTurn = function;
        return this;
    }

    public LuaAbilityBuilder OnUse(LuaFunction function)
    {
        _onUse = function;
        return this;
    }

    public LuaAbilityBuilder CanUse(LuaFunction function)
    {
        _canUse = function;
        return this;
    }

    public LuaAbilityBuilder OnTurnEnd(LuaFunction function)
    {
        _onTurnEnd = function;
        return this;
    }

    public IAbility Build()
    {
        var ability = new LuaAbility(_onReceiveDamage, _onDealDamage, _onTargetWithSpell, _onTargetedBySpell,
            _onCalculateStats, _onNextTurn, _onUse, _canUse, _onTurnEnd);
        _onAbilityBuild?.Invoke(ability);
        return ability;
    }
}