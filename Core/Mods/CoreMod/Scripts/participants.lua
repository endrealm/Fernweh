CreateParticipant("guard")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(15)
                    :Mana(1)
                    :AddAbility(abilityBuilder("test_ability"):Build())
                    :AddAbility(abilityBuilder("test_ability_2"):Build())
                    :Build();
        end)
        :Build();

CreateParticipant("tharmus")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(50)
                    :Mana(10)
                    :AddAbility(abilityBuilder("test_ability"):Build())
                    :AddAbility(abilityBuilder("test_ability_2"):Build())
                    :Build();
        end)
        :Build();