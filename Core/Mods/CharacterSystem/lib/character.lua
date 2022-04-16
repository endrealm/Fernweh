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
    }
end

function Character:Deserialize(data)
    self.items = data.items
    self.stats = data.stats
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
-- Battle
-- ============================

function Character:GenerateParticipant(builder, abilityBuilder)
    builder
            :Health(self.stats.health)
            :Mana(self.stats.mana)
            :Agility(self.stats.agility)
            :Strength(self.stats.strength)
            :Defense(self.stats.defense)
            :Intellect(self.stats.intellect)
            :Spirit(self.stats.spirit)
            :Evasion(self.stats.evasion)
    
    return builder
            :Build();
end

-- ============================
-- EXPORTS
-- ============================
context:CreateVar("Character", Character)