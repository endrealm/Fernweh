---
order: 97
---

# Create Participant

## Description
Creates a battle participent. This is used to create pre-defined entities, usually enemies.

=== Perameters
 - `name` (string)
=== Instantiate Properties
 - `builder` ([?]())
 - `abilityBuilder` ([AbilityBuilder](/api/classes/ability-builder/))
===

## Example
```lua
CreateParticipant("test")
        :Instantiate(function(builder, abilityBuilder)
            return builder
                    :Health(20)
                    :Mana(10)
                    :AddAbility(abilityBuilder("test_ability"):Build())
                    :AddAbility(abilityBuilder("test_ability_2"):Build())
                    :Build();
        end)
        :Build();
```