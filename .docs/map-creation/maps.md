---
Order: 99
---

# Maps

Maps are used to load a series of tiles. You can have many, and switch between them. For example you might have a world map, that connects to a dungeon map when you enter a crypt. ``Fernweh`` has world culling, so you can easily load maps containing more than a million tiles, just be aware you will get higher loading times for such large maps.

```json
{
  "name": "testMap",
  "explorable": true,
  "tilePositions": {
    "0": {
      "0": {
        "name": "grass",
        "firstEnterState": "first_enter_grass",
        "enterState": "enter_grass",
        "leaveState": "leave_grass",
        "lastLeaveState": "last_leave_grass"
      }
    }
  }
}
```

=== Name
A way to reference the map from lua scripts.
=== Explorable
Defines whether or not the world renderer will show a Fog of War for unexplored areas. Might be unchecked for towns, or for story related purposes.
=== Tile Positions
A list of every single tile in the map... Hopefully this will be made easier for editing in the future :grimacing:.

- ``position`` - can be edited by changing the 0's, the first which is X and second Y
- ``name`` - the name of the tile to be used

- ``firstEnterState`` - the lua state to be called when you enter this tile from another (different)
- ``enterState`` - the lua state to be called every time you enter this tile except the first
- ``leaveState`` the lua state to be called every time you leave this tile except the last
- ``lastLeaveState`` the lua state to be called when when you enter another tile from this one (different)

You can leave a state variable blank, then the game will ignore it.
===

## Conclusion

Now you should be able to load your mod into the game and create a new game on it. If you copied the code from our examples, youll see youself standing on a large map consisting of exactly one grass tile :stuck_out_tongue:. 

Now it's time to start creating some lua scripts in order to add functionaility to these tiles and add an objective to your game!