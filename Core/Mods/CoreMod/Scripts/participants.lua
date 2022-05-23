CreateParticipant("guard")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(15)
                    :Mana(5)
                    :AddAbility(abilityBuilder("test_ability"):Build())
                    :AddAbility(abilityBuilder("test_ability_2"):Build())
                    :Build();
        end)
        :Build();

CreateParticipant("bandit")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(15)
                    :Mana(5)
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

CreateParticipant("rat")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(10)
                    :Mana(0)
                    :Build();
        end)
        :Build();

CreateParticipant("large_rat")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(25)
                    :Mana(0)
                    :Build();
        end)
        :Build();

CreateParticipant("gaint_weasel")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(30)
                    :Mana(0)
                    :Build();
        end)
        :Build();

CreateParticipant("wolf")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(20)
                    :Mana(0)
                    :Build();
        end)
        :Build();

CreateParticipant("kobold_melee")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(20)
                    :Mana(0)
                    :Build();
        end)
        :Build();

CreateParticipant("kobold_ranged")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(20)
                    :Mana(0)
                    :Build();
        end)
        :Build();