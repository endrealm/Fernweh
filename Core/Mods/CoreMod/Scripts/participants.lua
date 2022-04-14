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