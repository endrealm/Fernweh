CreateAbility("test_ability")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(3)
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

CreateConstantAbility("test_ability_2")
        :OnUse(
        function(context)
            context:QueueAction(BattleAction:CreateDamage({
                Damage = 10,
                Element = 1
            }, context.Participant, context.Targets))
        end)
        :Build();