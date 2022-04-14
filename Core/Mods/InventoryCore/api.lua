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

function Item:ShowOptions(renderer, context)
    renderer:AddText("inventory.item.noActions")
end


-- ============================
-- INVENTORY UI
-- ============================

noInventoryStates = {}

function BlackListState(stateId)
    table.insert(noInventoryStates, stateId)
end

BlackListState("ui_inventory")
BlackListState("ui_item_details")

StateBuilder("ui_inventory")
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Brown")
                renderer:AddAction(function() context:ChangeState(oldState) end, "inventory.close")
                renderer:AddText("inventory.header")
                for key, entry in ipairs(GetInventory()) do
                    renderer:AddAction(function()
                        activeDetailItem=entry
                        context:ChangeState("ui_item_details")
                    end, "inventory.item", { { "item", entry.item.id }, { "amount", tostring(entry.amount) } } )
                end
            end
        )
        :Build()

StateBuilder("ui_item_details")
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Brown")
                renderer:AddAction(function() context:ChangeState("ui_inventory") end, "inventory.item.close")
                renderer:AddText("inventory.item.header", { { "item", activeDetailItem.item.id }, { "amount", tostring(activeDetailItem.amount) } })
                activeDetailItem.item:ShowOptions(renderer, context)
            end
        )
        :Build()

local function has_value (tab, val)
    for index, value in ipairs(tab) do
        if value == val then
            return true
        end
    end

    return false
end

Global:AddOnPostStateRender(
        function(renderer, context)
            if(has_value(noInventoryStates, context.ActiveStateId)) then
                return
            end
            
            oldState = context.ActiveStateId;
            renderer:AddAction(function() context:ChangeState("ui_inventory") end, "inventory.open")
        end
)

-- ============================
-- EXPORTS
-- ============================
Context:CreateFunction("AddItem", AddItem)
Context:CreateFunction("GetInventory", GetInventory)
Context:CreateFunction("RegisterItem", RegisterItem)
Context:CreateVariable("Item", Item)
