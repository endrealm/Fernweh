﻿currentParty = {}
maxPartySize = 4
-- ============================
-- Methods
-- ============================

function IsInParty(character)
    if(character.id == nil) then
        return false, "invalid_character"
    end
    
    for _, value in ipairs(currentParty) do
        if(value.id == character.id) then
            return true
        end
    end
    
    return false
end

function AddToParty(character)
    if(character.id == nil) then
        return false, "invalid_character"
    end
    if(#currentParty >= maxPartySize) then
        return false, "party_full"
    end
    if(IsInParty(character)) then
        return false, "already_added"
    end
    table.insert(currentParty, character)
    return true
end


function RemoveFromParty(character)
    if(character.id == nil) then
        return false, "invalid_character"
    end
    local selected = -1
    for index, value in ipairs(currentParty) do
        if(value.id == character.id) then
            selected = index
        end
    end
    
    if(selected == -1) then
        return false, "not_in_party"
    end
    
    table.remove(currentParty, selected)
    return true
end

-- ============================
-- Events
-- ============================
RegisterFriendlyParticipantsProvider(function(builder, abilityBuilder)
    local participants = {}
    for _, character in pairs(currentParty) do
        table.insert(participants, character:GenerateParticipant(builder, abilityBuilder))
    end
    return participants
    -- return {
    --     builder("Yennifer")
    --         :Health(1000)
    --         :Mana(1000)
    --         :AddAbility(abilityBuilder("test_ability"):Build())
    --         :AddAbility(abilityBuilder("test_ability_2"):Build())
    --         :Build(),
    --     builder("Triss")
    --         :Health(1000)
    --         :Mana(1000)
    --         :AddAbility(abilityBuilder("test_ability"):Build())
    --         :AddAbility(abilityBuilder("test_ability_2"):Build())
    --         :Build(),
    --     builder("Geralt")
    --         :Health(1000)
    --         :Mana(1000)
    --         :AddAbility(abilityBuilder("test_ability"):Build())
    --         :AddAbility(abilityBuilder("test_ability_2"):Build())
    --         :Build(),
    --     builder("Ciri")
    --         :Health(1000)
    --         :Mana(1000)
    --         :AddAbility(abilityBuilder("test_ability"):Build())
    --         :AddAbility(abilityBuilder("test_ability_2"):Build())
    --         :Build(),
    -- }
end)

-- ============================
-- Exports
-- ============================
Context:CreateFunc("AddToParty", AddToParty)
Context:CreateFunc("IsInParty", IsInParty)
Context:CreateFunc("RemoveFromParty", RemoveFromParty)