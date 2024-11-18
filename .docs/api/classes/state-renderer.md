# StateRenderer
## Description
Is used to render to the scene
## Methods

### SetMode(mode)
=== Description
Change render mode for the following render calls (AddText, AddAction). Not case sensitive. Defaults to typewriter on invalid data.
==- Properties
 - `mode` (string) - either "static" or "typewriter"
==- Returns:
[StateRenderer]()
===

### AddLabel(x, y, key, [replacements])
=== Description
Adds a screen label. Will be cleared with text.
==- Properties
 - `x` (number) - x coordinate inside game view relative to screen left.
 - `y` (number) - y coordinate inside game view relative to screen top (positive values downwards).
 - `key` (string) - text translation key
 - `replacements` - ([Replacements](/scripting/replacements.md))
==- Returns:
[StateRenderer]()
===

### AddText(key, [replacements])
=== Description
Add text to text view
==- Properties
 - `key` (string) - text translation key
 - `replacements` - ([Replacements](/scripting/replacements.md))
==- Returns:
[StateRenderer]()
===

### AddAction(callback, key, [replacements])
=== Description
Add text to text view
==- Properties
 - `callback` (LuaFunction) - no param lua function that is invoked when the text is clicked
 - `key` (string) - text translation key
 - `replacements` - ([Replacements](/scripting/replacements.md))
==- Returns:
[StateRenderer]()
===

### SetBackgroundColor(color)
=== Description
Sets the background color for the current
==- Properties
 - `color` ([Color](/scripting/color.md))
==- Returns:
[StateRenderer]()
===
