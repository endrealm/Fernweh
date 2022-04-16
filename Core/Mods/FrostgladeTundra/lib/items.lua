-- ============================
-- Imports
-- ============================

local equip = Import("character_system", "lib/equip")
local EquipItem = equip:Get("EquipItem")

local inventory = Import("inventory", "api")
local RegisterItem = inventory:GetFunc("RegisterItem")

-- ============================
-- Items
-- ============================

RegisterItem(EquipItem:new({
    id = "infinity_blade",
    tags = {"equip"},
    abilities = {
        test_ability = {},
    },
}))