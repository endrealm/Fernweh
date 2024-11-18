# AbilityBuilder

## Description
Allows to build abilities that can be cast in battles.
## Methods

### OnReceiveDamage(function)
=== Description
Is called when the player with the ability receives damage.
==- Properties
 - `function` ([ReceiveDamageFunction](../functions/receive-damage.md))
==- Returns:
[AbilityBuilder]()
===

### OnDealDamage(function)
=== Description
Is called when the player with the ability deals damage.
==- Properties
 - `function` ([DealDamageFunction](../functions/deal-damage.md))
==- Returns:
[AbilityBuilder]()
===

### OnTargetWithSpell(function)
=== Description
Is called when the player with the ability targets someone with any spell.
==- Properties
 - `function` ([TargetWithSpellFunction](../functions/target-with-spell.md))
==- Returns:
[AbilityBuilder]()
===

### OnTargetedBySpell(function)
=== Description
Is called when the player with the ability is targeted by someone with any spell.
==- Properties
 - `function` ([TargetedBySpellFunction](../functions/targeted-by-spell.md))
==- Returns:
[AbilityBuilder]()
===

### OnCalculateStats(function)
=== Description
Is called when the stats of the player are calculated. This might be called frequently.
==- Properties
 - `function` ([CalculateStatsFunction](../functions/calculate-stats.md))
==- Returns:
[AbilityBuilder]()
===

### OnNextTurn(function)
=== Description
Is called when next turn starts
==- Properties
 - `function` ([NextTurnFunction](../functions/next-turn.md))
==- Returns:
[AbilityBuilder]()
===

### AbilityOnUse(function)
=== Description
Is called when the ability used and handles the main part of your ability logic
==- Properties
 - `function` ([UseFunction](../functions/ability-use.md))
==- Returns:
[AbilityBuilder]()
===

### AbilityCanUse(function)
=== Description
Helps to define if the ability currently can be cast or not. If not the ability will be hidden in the ability selection.
==- Properties
 - `function` ([CanUseFunction](../functions/ability-can-use.md))
==- Returns:
[AbilityBuilder]()
===

### OnTurnEnd(function)
=== Description
Is called when the turn ends
==- Properties
 - `function` ([EndTurnFunction](../functions/turn-end.md))
==- Returns:
[AbilityBuilder]()
===

### CategoryId(categoryId)
=== Description
Defines the category the ability is grouped in menus. Defaults to `ability` if not set.
==- Properties
 - categoryId (string) the category
==- Returns:
[AbilityBuilder]()
===

### ManaCost(cost)
=== Description
Sets the amount of mana required to use the function.
==- Properties
 - cost (int) must be >= 0
==- Returns:
[AbilityBuilder]()
===

### TargetType(type)
=== Description
The target type. Values are:
 - 0 = FriendlySingle
 - 1 = FriendlyGroup
 - 2 = EnemyGroup
 - 3 = EnemySingle
 - 4 = EnemyAll
 - 5 = All
==- Properties
 - type (int) the target type
==- Returns:
[AbilityBuilder]()
===

### AllowDeadTargets(allow)
=== Description
Used to define if this ability can target dead participants. If not set the ability will not allow targeting dead.
==- Properties
 - `allow` (bool) allow to target dead participants
==- Returns:
[AbilityBuilder]()
===

### AllowLivingTargets(allow)
=== Description
Used to define if this ability can target living participants. If not set the ability will allow targeting living.
==- Properties
 - `allow` (bool) allow to target living participants
==- Returns:
[AbilityBuilder]()
===


### Build()
=== Description
Builds the ability to a useable format.
==- Returns:
[IAbility](../intermediate/iability.md)
===