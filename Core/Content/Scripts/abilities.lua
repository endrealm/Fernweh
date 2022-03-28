CreateAbility("test_ability")
        :Instantiate(
        function(builder, config)
            return builder
                    :OnUse(
                    function(context)
                        print("Ability used!")
                    end)
                    :Build();

        end)
        :Build();

CreateConstantAbility("test_ability_2")
        :OnUse(
        function(context)
            context:QueueAction(BattleAction:CreateDamage({
                Damage = 10,
                Element = 1
            }, context.Participant, context.Targets))
        end)
        :Build();