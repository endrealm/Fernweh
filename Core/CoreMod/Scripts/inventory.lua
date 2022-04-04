-- ============================
-- INVENTORY
-- ============================

inventory = {}

function AddItem(item)
    local id;

    if(type(item) == "string") then
        id = item
    else
        id = item.id
    end
    
    local amount = inventory[id]
    if(amount == nil) then
        amount = 0;
    end
    inventory[id] = amount +1;
end

function GetInventory()
    local items = {}

    for key, value in pairs(inventory) do
        table.insert(items, {
            item = itemRegistry[key],
            amount = value,
        })
    end
    
    return items
end

-- ============================
-- ITEM REGISTRY
-- ============================
itemRegistry = {}

function RegisterItem(item)
    itemRegistry[item.id] = item;
end

-- ============================
-- ITEM CLASS
-- ============================
Item = {}
---@param o {id: string}
function Item:new(o)
    if(o == nil) then
        error("nil properties passed to item constructor")
    end

    if(o.id == nil) then
        error("Property id not set")
    end
    
    setmetatable(o, self)
    self.__index = self
    return o
end



-- ============================
-- TEMPORARY TESTING
-- ============================
RegisterItem(Item:new{id = "sample_item"})

StateBuilder("my_other_state")
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Brown")
                renderer:AddText("inventory.header")
                renderer:AddAction(function() context:Exit() end, "inventory.close")
                for entry in GetInventory() do
                    renderer:AddText("inventory.item", {"item", entry.item.id}, {"amount", tostring(entry.amount)})
                end
            end
        )
        :Build()