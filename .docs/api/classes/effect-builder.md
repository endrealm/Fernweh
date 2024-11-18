# LuaEffectBuilder

## Description
Allows to build status effects that can be inflicted in battles
## Methods

### OnReceiveDamage(function)
=== Description
Is called when the player with the status effect receives damage.
==- Properties
 - `function` ([ReceiveDamageFunction](../functions/receive-damage.md))
==- Returns:
[LuaEffectBuilder]()
===

### OnDealDamage(function)
=== Description
Is called when the player with the status effect deals damage.
==- Properties
 - `function` ([DealDamageFunction](../functions/deal-damage.md))
==- Returns:
[LuaEffectBuilder]()
===

### OnTargetWithSpell(function)
=== Description
Is called when the player with the status effect targets someone with any spell.
==- Properties
 - `function` ([TargetWithSpellFunction](../functions/target-with-spell.md))
==- Returns:
[LuaEffectBuilder]()
===

### OnTargetedBySpell(function)
=== Description
Is called when the player with the status effect is targeted by someone with any spell.
==- Properties
 - `function` ([TargetedBySpellFunction](../functions/targeted-by-spell.md))
==- Returns:
[LuaEffectBuilder]()
===

### OnCalculateStats(function)
=== Description
Is called when the stats of the player are calculated. This might be called frequently.
==- Properties
 - `function` ([Stats](./stats.md))
==- Returns:
[LuaEffectBuilder]()
===

### OnNextTurn(function)
=== Description
Is called when next turn starts
==- Properties
 - `function` ([NextTurnFunction](../functions/next-turn.md))
==- Returns:
[LuaEffectBuilder]()
===

### OnTryCleanse(function)
=== Description
Is called when the theres an attempt at cleansing the status effect
==- Properties
 - `function` ([TryCleanseFunction](../functions/try-cleanse.md))
==- Returns:
[LuaEffectBuilder]()
===

### OnTurnEnd(function)
=== Description
Is called when the current turn ends
==- Properties
 - `function` ([TurnEndFunction](../functions/turn-end.md))
==- Returns:
[LuaEffectBuilder]()
===

### Build()
=== Description
Builds the effect to a useable format
==- Returns:
[IStatusEffect](../intermediate/istatuseffect.md)
===