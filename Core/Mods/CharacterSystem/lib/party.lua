RegisterFriendlyParticipantsProvider(function(builder, abilityBuilder) 
    return {
        builder("Yennifer")
            :Health(1000)
            :Mana(1000)
            :AddAbility(abilityBuilder("test_ability"):Build())
            :AddAbility(abilityBuilder("test_ability_2"):Build())
            :Build(),
        builder("Triss")
            :Health(1000)
            :Mana(1000)
            :AddAbility(abilityBuilder("test_ability"):Build())
            :AddAbility(abilityBuilder("test_ability_2"):Build())
            :Build(),
        builder("Geralt")
            :Health(1000)
            :Mana(1000)
            :AddAbility(abilityBuilder("test_ability"):Build())
            :AddAbility(abilityBuilder("test_ability_2"):Build())
            :Build(),
        builder("Ciri")
            :Health(1000)
            :Mana(1000)
            :AddAbility(abilityBuilder("test_ability"):Build())
            :AddAbility(abilityBuilder("test_ability_2"):Build())
            :Build(),
    }
end)