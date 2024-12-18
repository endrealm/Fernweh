﻿namespace Core.Scenes.Ingame.Battle;

public interface IBattleEventReceiver
{
    void OnReceiveDamage(DamageReceiveEvent evt);
    void OnDealDamage(DamageDealEvent evt);
    void OnTargetWithSpell(SpellTargetEvent evt);
    void OnTargetedBySpell(SpellTargetEvent evt);
    void OnCalculateStats(Stats stats);
    void OnNextTurn(out bool skip);
    void OnTurnEnd();
}