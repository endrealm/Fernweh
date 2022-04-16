local inventory = Import("inventory", "api")
local Item = inventory:Get("Item")
EquipItem = {}

function EquipItem:new(o)
    self.__index = self
    setmetatable(self, {__index = Item})
    local obj = Item:new(o)
    setmetatable(obj, self)

    -- custom init code here
    
    return obj
end

function EquipItem:ShowOptions(renderer, context)
    renderer:AddAction("inventory.item.equip", function() 
        
    end)
end

Context:CreateVar("EquipItem", EquipItem)