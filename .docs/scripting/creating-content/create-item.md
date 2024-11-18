---
order: 99
---

# Create Item

## Description
Creates and adds an item to the game

## Example
```lua
-- Imports
local equip = Import("character_system", "lib/equip")
local RegisterSlot = equip:GetFunc("RegisterSlot")
local EquipItem = equip:Get("EquipItem")

local inventory = Import("inventory", "api")
local Item = inventory:Get("Item")
local RegisterItem = inventory:GetFunc("RegisterItem")
local AddItem = inventory:GetFunc("AddItem")

-- Register different equipment slots. This is needed for creating equip items
RegisterSlot("weapon")
RegisterSlot("head")
RegisterSlot("body")
RegisterSlot("feet")
RegisterSlot("accessory")

-- Equip Items
RegisterItem(EquipItem:new({
    id = "infinity_blade",
    slot="weapon",
    abilities = {
        test_ability = {}, -- Give wearer access to test_ability
        stats = { -- Give stat upgrades to wearer
            strength=8,
            dexterity=3,
        },
    },
}))

-- Consumable Items
RegisterItem(Item:new({
    id = "health_potion",
    price= 10,
    sellPrice= 5,
    consumableAbility = {
        health_potion = {},
    },
}))

AddItem("health_potion", 3) -- this would add 3 potions to the players inventory
```