-- ============================
-- Imports
-- ============================

local equip = Import("character_system", "lib/equip")
local EquipItem = equip:Get("EquipItem")

local inventory = Import("inventory", "api")
local RegisterItem = inventory:GetFunc("RegisterItem")
local AddItem = inventory:GetFunc("AddItem")

-- ============================
-- Items
-- ============================

RegisterItem(EquipItem:new({
    id = "infinity_blade",
    slot="weapon",
    abilities = {
        test_ability = {},
    },
}))

RegisterItem(EquipItem:new({
    id = "lethality_blade",
    slot="weapon",
    price= 15,
    sellPrice= 15,
    abilities = {
        test_ability = {},
    },
}))

AddItem("infinity_blade", 3)
AddItem("lethality_blade", 2)