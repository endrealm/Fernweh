-- ============================
-- Imports
-- ============================

local inventory = Import("inventory", "api")
local GetItem = inventory:GetFunc("GetItem")

-- ============================
-- Core
-- ============================
Character = {}

function Character:new(o)
    if(o == nil) then
        error("nil properties passed to item constructor")
    end
    
    if(o.id == nil) then
        error("Property id not set")
    end
    
    o.type = o.type or "default"
    o.items = o.items or {}
    o.equip = o.equip or {}
    o.stats = o.stats or {}
    o.current = o.current or {}
    
    -- Init base stats
    o.stats.health = o.stats.health or 10
    o.stats.mana = o.stats.mana or 1
    o.stats.armor = o.stats.armor or 0
    o.stats.experience = o.stats.experience or 0
    o.stats.dexterity = o.stats.dexterity or 1
    o.stats.strength = o.stats.strength or 1
    o.stats.constitution = o.stats.constitution or 1
    o.stats.intellect = o.stats.intellect or 1
    o.stats.wisdom = o.stats.wisdom or 1
    o.stats.charisma = o.stats.charisma or 1
    
    setmetatable(o, self)
    self.__index = self
    return o
end

function Character:SetCurrentHealth(health)
    self.current.health = health;
end
function Character:SetCurrentMana(mana)
    self.current.mana = mana;
end

function Character:GetItemStats()
    local stats = {}
    for i, item in ipairs(self:GetEquip()) do
        local itemStats = item:GetApproximateStats();

        for statKey, statValue in pairs(itemStats) do
            stats[statKey] = (stats[statKey] or 0) + statValue;
        end
    end
    
    return stats
end

-- ============================
-- Data Serialization
-- ============================

function Character:Serialize()
    return {
        items = self.items,
        stats = self.stats,
        equip = self.equip,
        current = self.current,
    }
end

function Character:DisplayName()
    return GetTranslation("character."..self.id..".name")
end


function Character:Deserialize(data)
    self.items = data.items;
    self.stats = data.stats;
    self.equip = data.equip;
    self.current = data.current;
end

-- ============================
-- Items
-- ============================

function Character:AddItem(item, amount)
    local id;

    if(type(item) == "string") then
        id = item
    else
        id = item.id
    end

    self.items[id] = (self.items[id] or 0) + (amount or 1);
end

function Character:GetItems()
    local items = {}

    for key, value in pairs(self.items) do
        table.insert(items, {
            item = GetItem(key),
            amount = value,
        })
    end

    return items
end

-- ============================
-- Equip
-- ============================
function Character:EquipItem(item, slot)
    local id;

    if(type(item) == "string") then
        id = item
    else
        id = item.id
    end

    self.equip[slot] = id;
end

function Character:GetEquip()
    local items = {}

    for key, value in pairs(self.equip) do
        if(value ~= nil) then
            table.insert(items, GetItem(value))
        end
    end

    return items
end

function Character:UnequipItem(slot)
    self.equip[slot] = nil;
end

function Character:GetEquippedItem(slot)
    return GetItem(self.equip[slot]);
end

-- ============================
-- Battle
-- ============================

function Character:GenerateParticipant(createBuilder, abilityBuilder)
    local builder = createBuilder(self.id)
    builder
            :Health(self.stats.health)
            :Mana(self.stats.mana)
            :Armor(self.stats.armor)
            :Experience(self.stats.experience)
            :Dexterity(self.stats.dexterity)
            :Strength(self.stats.strength)
            :Constitution(self.stats.constitution)
            :Intellect(self.stats.intellect)
            :Wisdom(self.stats.wisdom)
            :Charisma(self.stats.charisma);

    -- Check if we have a different current health value
    if(self.current.health ~= nil) then
        builder:CurrentHealth(self.current.health)
    end
    -- Check if we have a different current mana value
    if(self.current.mana ~= nil) then
        builder:CurrentMana(self.current.mana)
    end

    -- Load abilities from held items
    for _, value in ipairs(self:GetItems()) do
        local abilities = value.item:ParseAbility(abilityBuilder)
        if(abilities ~= nil) then
            for _, ability in ipairs(abilities) do
                builder:AddAbility(ability)
            end
        end
    end
    
    -- Load abilities from equipped items
    for _, value in ipairs(self:GetEquip()) do
        local abilities = value:ParseAbility(abilityBuilder)
        if(abilities ~= nil) then
            for _, ability in ipairs(abilities) do
                builder:AddAbility(ability)
            end
        end
    end
    
    return builder
            :Build();
end

-- ============================
-- EXPORTS
-- ============================
Context:CreateVar("Character", Character)