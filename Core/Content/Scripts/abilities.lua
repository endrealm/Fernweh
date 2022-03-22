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
            print("Ability used!")
        end)
        :Build();