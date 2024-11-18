---
order: 100
---

# Create Ability

## Description
Creates an ability that can be equiped and used. This ability can change depending on context.

### Example
```lua
CreateAbility("test_ability")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(3) -- Targets single enemy
                    :ManaCost(2)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:CreateDamage({
                            Damage = 6,
                            Element = 1
                        }, context.Participant, context.Targets))
                    end)
                    :Build();

        end)
        :Build();
```

||| Target types:

0. FriendlySingle
1. FriendlyGroup
1. EnemySingle
1. EnemyGroup
1. EnemyAll
1. All

||| Element types:

0. None
1. Ice
1. Air
1. Ground
1. Thunder
1. Water
1. Fire
|||

## Create Constant Ability
Creates an ability that does the exact same thing each time. Used for situations, like reviving, or giving status effects.

### Example
```lua
CreateConstantAbility("test_ability_2")
        :OnUse(
        function(context)
            context:QueueAction(BattleAction:CreateDamage({
                Damage = 10,
                Element = 1
            }, context.Participant, context.Targets))
        end)
        :Build();
```