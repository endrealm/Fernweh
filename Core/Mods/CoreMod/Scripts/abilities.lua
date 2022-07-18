CreateAbility("fire")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(2)
                    :ManaCost(3)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:CreateDamage({
                            Damage = context.Participant:GetStats().Intellect + 5,
                            Element = 1
                        }, context.Participant, context.Targets))
                    end)
                    :Build();
        end)
        :Build();

CreateAbility("blizzard")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(4)
                    :ManaCost(4)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:CreateDamage({
                            Damage = context.Participant:GetStats().Intellect,
                            Element = 1
                        }, context.Participant, context.Targets))
                    end)
                    :Build();
        end)
        :Build();

CreateAbility("heal")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(0)
                    :ManaCost(3)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:HealHealth({
                            Health = context.Participant:GetStats().Intellect
                        }, context.Participant, context.Targets))
                    end)
                    :Build();
        end)
        :Build();

CreateAbility("health_potion")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(0)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:HealHealth({
                            Health = 20
                        }, context.Participant, context.Targets))
                    end)
                    :Build();
        end)
        :Build();

CreateAbility("ether")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(0)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:HealMana({
                            Mana = 15
                        }, context.Participant, context.Targets))
                    end)
                    :Build();
        end)
        :Build();

CreateAbility("revive")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(0)
                    :AllowDeadTargets(true)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:HealHealth({
                            Health = 15
                        }, context.Participant, context.Targets))
                    end)
                    :Build();
        end)
        :Build();

CreateAbility("power_stab")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(2)
                    :ManaCost(2)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:CreateDamage({
                            Damage = context.Participant:GetStats().Intellect + 5,
                            Element = 0
                        }, context.Participant, context.Targets))
                    end)
                    :Build();
        end)
        :Build();

CreateAbility("spinning_slash")
        :Instantiate(
        function(builder, config)
            return builder
                    :TargetType(4)
                    :ManaCost(4)
                    :OnUse(
                    function(context)
                        context:QueueAction(BattleAction:CreateDamage({
                            Damage = context.Participant:GetStats().Intellect,
                            Element = 0
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