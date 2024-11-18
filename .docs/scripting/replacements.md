---
order: 78
---

# Text Replacements
Text replacements allow you to dynamically change and interact with text. You can switch out placeholders to create more dynamic text.

Imagine you have the following translation:
```json
{
    "example.text": {
        "translations": {
            "EN_US": "Greetings {name}!"
        }
    }
}
```
We now would like to exchange `{name}`. Replacement text is always surrounded by curly brackets! If you don't replace it it will remain in your text. Now in your state render method you can render your text:
```lua
renderer:AddText("example.text", {{"name", "Harrald"}})
```
As you can see the second parameter is a lua array with lua arrays. The first value in the inner array is the key to replace alias `name` in our case. The second value is what we would like to add in there. In this case we are replacing it with a plain string, but we could also put in another translation:
```lua
local npcName = GetTranslation("npc.harrald.name") -- note you would have to add a translation here
renderer:AddText("example.text", {{"name", npcName}})
```
Now you can color the name in another translation and inject it into the first one.
You can also add more by adding more elements:
```json
{
    "example.text": {
        "translations": {
            "EN_US": "Greetings {name} the {role}!"
        }
    },
    "npc.harrald.name": {
        "translations": {
            "EN_US": "<color=\"Orange\">Harrald</color>"
        }
    },
    "npc.harrald.role": {
        "translations": {
            "EN_US": "<color=\"Black\">Blacksmith</color>"
        }
    },
}
```
```lua
local npcName = GetTranslation("npc.harrald.name")
local npcRole = GetTranslation("npc.harrald.role")
renderer:AddText("example.text", {{"name", npcName}, {"role", npcRole}})
```