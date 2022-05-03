-- ============================
-- Imports
-- ============================

local registry = Import("lib/registry")
local LoadCharacterType = registry:GetFunc("LoadCharacterType")

-- ============================
-- Data
-- ============================

currentParty = {}
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

function GetMembers()
    return currentParty;
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
end)

-- ============================
-- Save party
-- ============================

SetDataLoader(function(data)
    if(data == nil) then
        return
    end
    currentParty = {}
    for _, characterData in pairs(data) do
        table.insert(currentParty, LoadCharacterType(characterData.type, characterData.id, characterData.data))
    end
end)

SetDataSaver(function()
    local data = {}

    for _, character in pairs(currentParty) do
        table.insert(data, {
            type=character.type,
            id=character.id,
            data=character:Serialize(),
        })
    end

    return data
end)

-- ============================
-- Exports
-- ============================
Context:CreateFunc("AddToParty", AddToParty)
Context:CreateFunc("GetMembers", GetMembers)
Context:CreateFunc("IsInParty", IsInParty)
Context:CreateFunc("RemoveFromParty", RemoveFromParty)