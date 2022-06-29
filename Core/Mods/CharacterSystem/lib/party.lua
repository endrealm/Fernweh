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

function GetMemberById(id)
    for index, value in ipairs(currentParty) do
        if(id == value.id) then
            return value;
        end
    end
    return nil;
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

Global:AddOnPostBattle(function(victory, snapshot)
    --print("Starting party post battle save")
    for candidateCount = 0, snapshot.Friendlies.Count - 1 do
        local candidate = snapshot.Friendlies[candidateCount]
        local id = candidate.Config.Id
        local char = GetMemberById(id)
        if(char ~= nil) then
            char:SetCurrentHealth(candidate.Health)
            char:SetCurrentMana(candidate.Mana)

            -- calc gained xp
            local xp = 0
            for i=0,snapshot.Enemies.Count - 1 do
	            xp = xp + snapshot.Enemies[i].Config.Stats.Health
            end

            -- add xp, then level up if needed
            char.stats.experience = char.stats.experience + xp
            while(char.stats.experience >= char:GetExperienceForLevelUp() and char.stats.level < 100)
            do
                char.stats.level = char.stats.level + 1
                char.stats.experience = char.stats.experience - char:GetExperienceForLevelUp()

                -- stat upgrades
                char.stats.health = char.stats.health + 3
                char.stats.mana = char.stats.mana + 2
                char.current.health = char.current.health + 3
                char.current.mana = char.current.mana + 2
                char.stats.strength = char.stats.strength + 1
                char.stats.constitution = char.stats.constitution + 1
                char.stats.dexterity = char.stats.dexterity + 1
                char.stats.intellect = char.stats.intellect + 1
                char.stats.wisdom = char.stats.wisdom + 1
                char.stats.charisma = char.stats.charisma + 1
            end
        end
    end
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