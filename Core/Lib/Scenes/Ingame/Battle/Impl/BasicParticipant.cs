using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using Core.Scenes.Ingame.Battle.Impl.Strategy;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle.Impl;

public class BasicParticipant : IBattleParticipant
{
    private readonly List<IAbility> _abilities = new();
    private readonly ParticipantConfig _config;
    private readonly List<IStatusEffect> _effects = new();

    public BasicParticipant(string participantId, string groupId, ParticipantConfig config)
    {
        _config = config;
        ParticipantId = participantId;
        GroupId = groupId;
        var stats = GetStats();
        Health = config.Health < 0 ? stats.Health : Math.Min(stats.Health, config.Health);
        Mana = config.Mana < 0 ? stats.Mana : Math.Min(stats.Mana, config.Mana);
        State = Health > 0 ? ParticipantState.Alive : ParticipantState.Dead;
    }

    public string ParticipantId { get; }
    public string GroupId { get; }
    public string DisplayName => ParticipantId;

    public int Health { get; private set; }
    public ParticipantState State { get; private set; } = ParticipantState.Alive;

    public int Mana { get; private set; }
    public bool Defending { get; set; }

    public IBattleStrategy Strategy { get; set; } = new AggressiveStrategy();

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
            updateContext.QueueAction(new LogTextAction("battle.participant.death",
                new TextReplacement("name", DisplayName)));
            updateContext.SoundPlayer.PlaySFX("death");
            updateContext.QueueAction(new AwaitNextAction());
        }

        State = ParticipantState.Dead;
    }

    public int HealHealth(int health)
    {
        var amount = Math.Min(_config.Stats.Health, Health + health) - Health;
        Health = amount + Health;
        return amount;
    }

    public int HealMana(int mana)
    {
        var amount = Math.Min(_config.Stats.Mana, Mana + mana) - Mana;
        Mana = amount + Mana;
        return amount;
    }

    public void DeductMana(int mana)
    {
        Mana = Math.Max(0, Mana - mana);
    }

    public ParticipantSnapshot CreateSnapshot()
    {
        return new ParticipantSnapshot
        {
            Config = _config,
            Health = Health,
            Mana = Mana
        };
    }

    #region Events

    public void OnReceiveDamage(DamageReceiveEvent evt)
    {
        _effects.ForEach(e => e.OnReceiveDamage(evt));
        _abilities.ForEach(e => e.OnReceiveDamage(evt));
        if (Defending) evt.Data.Damage = (int) Math.Ceiling(evt.Data.Damage / 2f);
        if (evt.Data.Damage <= 0) return; // damage was somehow blocked
        Health = Math.Max(0, Health - evt.Data.Damage);
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
        if (evt.Data.ManaCost <= 0) return; // damage was somehow blocked
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
}