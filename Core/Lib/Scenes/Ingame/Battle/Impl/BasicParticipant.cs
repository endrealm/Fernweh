using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle.Impl;

public class BasicParticipant : IBattleParticipant
{
    private readonly ParticipantConfig _config;
    private readonly List<IAbility> _abilities = new();
    private readonly List<IStatusEffect> _effects = new();

    public BasicParticipant(string participantId, ParticipantConfig config)
    {
        _config = config;
        ParticipantId = participantId;
        Health = config.Health;
        Mana = config.Mana;
    }

    #region Events

    public void OnReceiveDamage(DamageReceiveEvent evt)
    {
        _effects.ForEach(e => e.OnReceiveDamage(evt));
        _abilities.ForEach(e => e.OnReceiveDamage(evt));
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
        _effects.ForEach(e => e.OnTurnEnd());
        _abilities.ForEach(e => e.OnTurnEnd());
    }

    #endregion

    public string ParticipantId { get; }

    public int Health { get; }

    public int Mana { get; }

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
}