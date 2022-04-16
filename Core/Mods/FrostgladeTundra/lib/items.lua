-- ============================
-- Imports
-- ============================

local inventory = Import("inventory", "api")
local Item = inventory:Get("Item")
local RegisterItem = inventory:GetFunc("RegisterItem")

-- ============================
-- Items
-- ============================
RegisterItem(Item:new({
    id = "infinity_blade",
    tags = {"equip"},
    abilities = {
        test_ability = {},
    },
}))