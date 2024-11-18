-- ============================
-- Imports
-- ============================

local registry = Import("lib/registry")
local LoadCharacterType = registry:GetFunc("LoadCharacterType")

local uiCompat = Import("ui_compat", "api")
BlackListState = uiCompat:GetFunc("BlackListState")

-- ============================
-- Data
-- ============================

currentParty = {}
maxPartySize = 4
-- ============================
-- Methods
-- ============================

function IsInParty(character)
    local name

    if (type(character) == "string") then
        name = character
    else
        name = character.id
    end

    for _, value in ipairs(currentParty) do
        if (value.id == name) then
            return true
        end
    end

    return false
end

function AddToParty(character)
    if (character.id == nil) then
        return false, "invalid_character"
    end
    if (#currentParty >= maxPartySize) then
        return false, "party_full"
    end
    if (IsInParty(character)) then
        return false, "already_added"
    end
    table.insert(currentParty, character)
    return true
end

function RemoveFromParty(character)
    if (character.id == nil) then
        return false, "invalid_character"
    end
    local selected = -1
    for index, value in ipairs(currentParty) do
        if (value.id == character.id) then
            selected = index
        end
    end

    if (selected == -1) then
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
        if (id == value.id) then
            return value;
        end
    end
    return nil;
end

-- ============================
-- Battle
-- ============================

local moneyReward
local lootReward
local partySnapshot
local xpGained = 0

function SetPostBattleInfo(money, loot)
    -- will show exp gain, leveling, and any other info after battle
    moneyReward = money
    lootReward = loot
end

BlackListState("post_battle_overview")
StateBuilder("post_battle_overview")
        :Render(
        function(renderer, context)
            if (xpGained == 0 or partySnapshot == nil) then
                -- exit state if no info (usually if you load a game into this state)
                renderer:AddText("")
                context:Exit()
            end

            renderer:AddText("battle.win")
            renderer:AddText("")

            renderer:AddText("battle.details.exp", { { "reward", xpGained } })
            if (moneyReward ~= nil) then
                renderer:AddText("battle.details.gold", { { "reward", moneyReward } })
            end
            if (lootReward ~= nil) then
                renderer:AddText("battle.details.loot")
                for _, l in ipairs(lootReward) do
                    renderer:AddText("battle.details.loot.item", { { "reward", l } })
                end
            end
            renderer:AddText("")

            for i, character in ipairs(GetMembers()) do
                -- if character and old character have the same level
                if (partySnapshot == nil or character.stats.level == partySnapshot[i]) then
                    renderer:AddText("battle.details.player.level", { { "name", character.id }, { "level", character.stats.level } })
                else
                    renderer:AddText("battle.details.player.levelup", { { "name", character.id }, { "level", character.stats.level }, { "old_level", partySnapshot[i] } })
                end
                renderer:AddText("battle.details.player.experience", { { "current", character.stats.experience }, { "max", character:GetExperienceForLevelUp() } })
            end

            renderer:AddAction(function()
                moneyReward = nil
                lootReward = nil
                partySnapshot = {}
                context:Exit()
            end, "button.continue")
        end
)
        :Build()

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
    -- save previous player levels
    partySnapshot = {}
    for i, character in ipairs(GetMembers()) do
        partySnapshot[i] = character.stats.level
    end

    -- calc gained xp
    for i = 0, snapshot.Enemies.Count - 1 do
        xpGained = xpGained + snapshot.Enemies[i].Config.Stats.Health
    end
    xpGained = xpGained / 2

    --print("Starting party post battle save")
    for candidateCount = 0, snapshot.Friendlies.Count - 1 do
        local candidate = snapshot.Friendlies[candidateCount]
        local id = candidate.Config.Id
        local char = GetMemberById(id)

        if (char ~= nil) then
            char:SetCurrentHealth(candidate.Health)
            char:SetCurrentMana(candidate.Mana)

            -- add xp, then level up if needed
            char.stats.experience = char.stats.experience + xpGained
            while (char.stats.experience >= char:GetExperienceForLevelUp() and char.stats.level < 100)
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
    if (data == nil) then
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
            type = character.type,
            id = character.id,
            data = character:Serialize(),
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
Context:CreateFunc("SetPostBattleInfo", SetPostBattleInfo)