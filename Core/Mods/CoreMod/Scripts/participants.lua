﻿CreateParticipant("guard")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(15)
                    :Mana(5)
                    :Strength(17)
                    :Constitution(15)
                    :Dexterity(12)
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
                    :Strength(15)
                    :Constitution(13)
                    :Dexterity(16)
                    :AddAbility(abilityBuilder("test_ability"):Build())
                    :AddAbility(abilityBuilder("test_ability_2"):Build())
                    :Build();
        end)
        :Build();

CreateParticipant("tharmus")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(90)
                    :Mana(10)
                    :Strength(20)
                    :Constitution(20)
                    :Dexterity(14)
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
                    :Strength(12)
                    :Constitution(5)
                    :Dexterity(16)
                    :Build();
        end)
        :Build();

CreateParticipant("large_rat")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(25)
                    :Mana(0)
                    :Strength(16)
                    :Constitution(13)
                    :Dexterity(14)
                    :Build();
        end)
        :Build();

CreateParticipant("gaint_weasel")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(30)
                    :Mana(0)
                    :Strength(17)
                    :Constitution(15)
                    :Dexterity(16)
                    :Build();
        end)
        :Build();

CreateParticipant("wolf")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(20)
                    :Mana(0)
                    :Strength(19)
                    :Constitution(13)
                    :Dexterity(16)
                    :Build();
        end)
        :Build();

CreateParticipant("kobold_melee")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(20)
                    :Mana(0)
                    :Strength(19)
                    :Constitution(17)
                    :Dexterity(14)
                    :Build();
        end)
        :Build();

CreateParticipant("kobold_ranged")
        :Instantiate(function(builder, abilityBuilder)
    
            return builder
                    :Health(20)
                    :Mana(0)
                    :Strength(17)
                    :Constitution(15)
                    :Dexterity(16)
                    :Build();
        end)
        :Build();