# GlobalEventHandler

## Description
Acts as an interface to subscribe to global events.
## Methods

### AddOnPrePlayerMove(function)
=== Description
Is called before the player is moved to the destination tile.
==- Properties
 - `function` ([PrePlayerMoveFunction](../functions/pre-player-move.md))
==- Returns:
[GlobalEventHandler]()
===

### AddOnPreStateChange(function)
=== Description
Is called before a state change us invoked
==- Properties
 - `function` ([PreStateChangeFunction](../functions/pre-state-change.md))
==- Returns:
[GlobalEventHandler]()
===

### AddOnPreStateRender(function)
=== Description
Is called before a state is rendered
==- Properties
 - `function` ([PreStateRenderFunction](../functions/pre-state-render.md))
==- Returns:
[GlobalEventHandler]()
===

### AddOnPostStateRender(function)
=== Description
Is called after a state is rendered
==- Properties
 - `function` ([PostStateRenderFunction](../functions/post-state-render.md))
==- Returns:
[GlobalEventHandler]()
===

### AddOnBattleSpriteLoad(function)
=== Description
Is called when battle sprites are to be loaded
==- Properties
 - `function` ([PostStateRenderFunction](../functions/battle-sprite-load.md))
==- Returns:
[GlobalEventHandler]()
===