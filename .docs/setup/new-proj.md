---
order: 99
---

# Create index.json

Before you can start programming, you need to create a workspace.

Create a folder for you mod and name it appropritaly (ex. "TestMod"). 
Inside you'll need to create a new file named `index.json`. For your mod to work, you'll need to fill this out with the correct data:

```json
{
  "id": "test_mod",
  "type": "game",
  "dependencies": [],
  "scripts": []
}
```

=== id
Will be used to reference the mod from inside lua (name)
=== type
Mods are catagorized as either a ``library`` or ``game``. Game mods are what you select when you make a new game, they include a campaign you can play through. While library mods are simply tools you can use like for adding inventories or quests.
=== dependancies
You can choose to use [mod libraries](/library-mods/) in your mod to easily add content. Each library mod listed here will be loaded with your mod.
=== scripts
This will list every lua script to be loaded in your mod.
===
