# LuaStateBuilder

## Description
Can be used to create new states.
## Methods

### Render(function)
=== Description
Is called when the state should be rendered/loaded. Should be used to select text and supply actions.
==- Properties
 - `function` ([RenderFunction](../functions/render.md))
==- Returns:
[LuaStateBuilder]()
===

### ShowExit(show)
=== Description
If set to true this will append an "exit" action at the end of the text body.
==- Properties
 - `show` (boolean)
==- Returns:
[LuaStateBuilder]()
===

### AllowSave(allow)
=== Description
If set to false this will prevent saving the state and fallback to last savable state on save. This might be useful for UI-States or state relying on non persistent data. Defaults to true.
==- Properties
 - `allow` (boolean)
==- Returns:
[LuaStateBuilder]()
===

### Sticky(sticky)
=== Description
If set to false this will prevent the chat from scrolling downwards automatically. Defaults to true.
==- Properties
 - `sticky` (boolean)
==- Returns:
[LuaStateBuilder]()
===

### ClearScreenPost(clear)
=== Description
If set to false this will preserve chat render content to the next state. Defaults to true.
==- Properties
 - `clear` (boolean)
==- Returns:
[LuaStateBuilder]()
===