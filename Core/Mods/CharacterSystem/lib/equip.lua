local inventory = Import("inventory", "api")
local Item = inventory:Get("Item")

local equipUI = Import("lib/ui/equip")
local SetEquipItem = equipUI:GetFunc("SetEquipItem")
-- ============================
-- Equip Item
-- ============================

EquipItem = {}
Slots = {}

function EquipItem:new(o)
    self.__index = self
    setmetatable(self, { __index = Item })
    local obj = Item:new(o)
    setmetatable(obj, self)

    -- custom init code here
    o.slot = o.slot or "head"

    return obj
end

function EquipItem:ShowOptions(renderer, context)
    renderer:AddAction(function()
        SetEquipItem(self)
        context:ChangeState("ui_equip_item")
    end, "inventory.item.equip")
end

function RegisterSlot(slot)
    if (type(slot) ~= "string") then
        error("Slot must be a string value")
    end
    table.insert(Slots, slot)
end

function GetSlots()
    return Slots
end


-- ============================
-- Exports
-- ============================
Context:CreateFunc("EquipItem", EquipItem)
Context:CreateFunc("RegisterSlot", RegisterSlot)
Context:CreateFunc("GetSlots", GetSlots)