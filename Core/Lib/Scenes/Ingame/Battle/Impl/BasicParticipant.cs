using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using Core.Scenes.Ingame.Chat;

namespace Core.Scenes.Ingame.Battle.Impl;

public class BasicParticipant : IBattleParticipant
{
    private readonly ParticipantConfig _config;
    private readonly List<IAbility> _abilities = new();
    private readonly List<IStatusEffect> _effects = new();

    public BasicParticipant(string participantId, string groupId, ParticipantConfig config)
    {
        _config = config;
        ParticipantId = participantId;
        GroupId = groupId;
        Health = (int)(config.Stats.Health*0.75f);
        Mana = (int)(config.Stats.Mana*0.75f);
    }

    #region Events

    public void OnReceiveDamage(DamageReceiveEvent evt)
    {
        _effects.ForEach(e => e.OnReceiveDamage(evt));
        _abilities.ForEach(e => e.OnReceiveDamage(evt));
        if(Defending)
        {
            evt.Data.Damage = (int)Math.Ceiling(evt.Data.Damage / 2f);
        }
        if(evt.Data.Damage <= 0) return; // damage was somehow blocked
        Health -= evt.Data.Damage;
    }

    public void OnDealDamage(DamageDealEvent evt)
    {
        _effects.ForEach(e => e.OnDealDamage(evt));
        _abilities.ForEach(e => e.OnDealDamage(evt));
    }

    public void OnTargetWithSpell(SpellTargetEvent evt)
    {
        _effects.ForEach(e => e.OnTargetWithSpell(evt));
        _abilities.ForEach(e => e.OnTargetWithSpell(evt));
        if(evt.Data.ManaCost <= 0) return; // damage was somehow blocked
        Mana -= evt.Data.ManaCost;
    }

    public void OnTargetedBySpell(SpellTargetEvent evt)
    {
        _effects.ForEach(e => e.OnTargetedBySpell(evt));
        _abilities.ForEach(e => e.OnTargetedBySpell(evt));
    }

    public void OnCalculateStats(Stats stats)
    {
        _effects.ForEach(e => e.OnCalculateStats(stats));
        _abilities.ForEach(e => e.OnCalculateStats(stats));
    }

    public void OnNextTurn(out bool skip)
    {
        var shouldSkipRet = false;
        _effects.ForEach(e =>
        {
            e.OnNextTurn(out var shouldSkip);
            if (shouldSkip) shouldSkipRet = true;
        });
        _abilities.ForEach(e =>
        {
            e.OnNextTurn(out var shouldSkip);
            if (shouldSkip) shouldSkipRet = true;
        });
        skip = shouldSkipRet;
    }

    public void OnTurnEnd()
    {
        Defending = false;
        _effects.ForEach(e => e.OnTurnEnd());
        _abilities.ForEach(e => e.OnTurnEnd());
    }

    #endregion

    public string ParticipantId { get; }
    public string GroupId { get; }
    public string DisplayName => ParticipantId;

    public int Health { get; private set; }
    public ParticipantState State { get; private set; } = ParticipantState.Alive;

    public int Mana { get; private set; }
    public bool Defending { get; set; }

    public IBattleStrategy Strategy { get; set; } = new FallbackStrategy();

    public List<IAbility> GetAbilities()
    {
        return _abilities;
    }

    public List<IStatusEffect> GetActiveEffects()
    {
        return _effects;
    }

    public IBattleAction NextAction { get; set; }
    public Stats GetStats()
    {
        var baseStats = _config.Stats.Clone();
        OnCalculateStats(baseStats);
        return baseStats;
    }

    public void UpdateParticipantState(ActionContext updateContext)
    {
        if (Health > 0)
        {
            State = ParticipantState.Alive;
            return;
        }
        Health = 0;
        if (State == ParticipantState.Alive)
        {
            updateContext.QueueAction(new LogTextAction("battle.participant.death", new Replacement("name", DisplayName)));
            updateContext.QueueAction(new AwaitNextAction());
        }
        State = ParticipantState.Dead;
    }
}