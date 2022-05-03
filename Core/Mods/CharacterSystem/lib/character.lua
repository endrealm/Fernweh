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
    
    -- Init base stats
    o.stats.health = o.stats.health or 1
    o.stats.mana = o.stats.mana or 1
    o.stats.agility = o.stats.agility or 1
    o.stats.strength = o.stats.strength or 1
    o.stats.defense = o.stats.defense or 1
    o.stats.intellect = o.stats.intellect or 1
    o.stats.spirit = o.stats.spirit or 1
    o.stats.evasion = o.stats.evasion or 1
    
    setmetatable(o, self)
    self.__index = self
    return o
end

-- ============================
-- Data Serialization
-- ============================

function Character:Serialize()
    return {
        items = self.items,
        stats = self.stats,
        equip = self.equip,
    }
end

function Character:DisplayName()
    return GetTranslation("character."..self.id..".name")
end


function Character:Deserialize(data)
    self.items = data.items
    self.stats = data.stats
    self.equip = data.equip
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

    self.equip[slot] = item;
end

function Character:GetEquip()
    local items = {}

    for key, value in pairs(self.equip) do
        if(value ~= nil) then
            table.insert(items, GetItem(key))
        end
    end

    return items
end

function Character:UnequipItem(slot)
    self.equip[slot] = nil;
end

function Character:GetEquippedItem(slot)
    return self.equip[slot];
end

-- ============================
-- Battle
-- ============================

function Character:GenerateParticipant(createBuilder, abilityBuilder)
    local builder = createBuilder(self.id)
    builder
            :Health(self.stats.health)
            :Mana(self.stats.mana)
            :Agility(self.stats.agility)
            :Strength(self.stats.strength)
            :Defense(self.stats.defense)
            :Intellect(self.stats.intellect)
            :Spirit(self.stats.spirit)
            :Evasion(self.stats.evasion)

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