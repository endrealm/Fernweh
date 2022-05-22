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
    private LuaFunction _onCalculateWeight;
    private LuaFunction _onTargetedBySpell;
    private LuaFunction _onCalculateStats;
    private LuaFunction _onNextTurn;
    private LuaFunction _onTurnEnd;
    private LuaFunction _onUse;
    private LuaFunction _canUse;
    private string _category = "ability";
    private int _manaCost = 0;
    private bool _allowDeadTargets = false;
    private bool _allowLivingTargets = true;
    private bool _hideBlocked = false;
    private bool _hidden = false;
    private AbilityTargetType _targetType = AbilityTargetType.EnemySingle;
    private readonly string _id;

    public LuaAbilityBuilder(string id, Action<LuaAbility> onAbilityBuild = null)
    {
        _id = id;
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

    public LuaAbilityBuilder OnCalculateWeight(LuaFunction function)
    {
        _onCalculateWeight = function;
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

    public LuaAbilityBuilder CategoryId(string category)
    {
        _category = category;
        return this;
    }

    public LuaAbilityBuilder ManaCost(int cost)
    {
        // Prevent negative mana amounts
        if (cost < 0) return this;
        _manaCost = cost;
        return this;
    }

    public LuaAbilityBuilder TargetType(int targetType)
    {
        _targetType = (AbilityTargetType)targetType;
        return this;
    }
    
    public LuaAbilityBuilder AllowDeadTargets(bool allow)
    {
        _allowDeadTargets = allow;
        return this;
    }
    
    public LuaAbilityBuilder HideBlocked(bool hide)
    {
        _hideBlocked = hide;
        return this;
    }
    public LuaAbilityBuilder Hidden(bool hide)
    {
        _hidden = hide;
        return this;
    }
    
    public LuaAbilityBuilder AllowLivingTargets(bool allow)
    {
        _allowLivingTargets = allow;
        return this;
    }

    public IAbility Build()
    {
        var ability = new LuaAbility(_onReceiveDamage, _onCalculateWeight, _onDealDamage, _onTargetWithSpell, _onTargetedBySpell,
            _onCalculateStats, _onNextTurn, _onUse, _canUse, _onTurnEnd, _category, _id, _manaCost, _targetType,
            _allowDeadTargets, _allowLivingTargets, _hideBlocked, _hidden);
        _onAbilityBuild?.Invoke(ability);
        return ability;
    }
}