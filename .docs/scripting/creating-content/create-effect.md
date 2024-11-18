---
order: 92
---

# Create Status Effect

## Description
Creates a status effect that can be applied to entities

=== Perameters
 - `name` (string)
=== Instantiate Properties
 - `effectBuilder` ([LuaEffectBuilder](/api/classes/effect-builder/))
 - `target` ([IBattleParticipant]())
  - `props` ([PropsArray]())
===

## Example
```lua
CreateStatusEffect("test")
    :Instantiate(
        function(effectBuilder, target, props)
            
            --local scoped variable here
            
            return effectBuilder
                :OnReceiveDamage(
                    function(event)

                    end
                ):OnDealDamage(
                    function(event)

                    end
                ):OnTargetWithSpell(
                    function(event)

                    end
                ):OnTargetedBySpell(
                    function(event)

                    end
                ):OnCalculateStats(
                    function(stats)
                        
                    end
                ):OnNextTurn(
                    function()
                        return false;
                    end
                ):OnTryCleanse(
                    function()
                        return false;
                    end
                )
        end
    )
    :Build();
```