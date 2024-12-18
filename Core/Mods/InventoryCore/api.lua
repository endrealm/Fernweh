﻿-- ============================
-- Imports
-- ============================
local uiCompat = Import("ui_compat", "api")
BlackListState = uiCompat:GetFunc("BlackListState")
IsUI = uiCompat:GetFunc("IsUI")
OldState = uiCompat:GetVar("OldState")

-- ============================
-- Stats approximation
-- ============================

local statsAbilityApproximationHandlers = {}

function AddStatApproximationHandler(callback)
    table.insert(statsAbilityApproximationHandlers, callback)
end

-- ============================
-- INVENTORY
-- ============================
inventory = {}

function AddItem(item, add)
    local id;

    if (add == nil) then
        add = 1
    end

    if (type(item) == "string") then
        id = item
    else
        id = item.id
    end

    local amount = inventory[id]
    if (amount == nil) then
        amount = 0;
    end
    inventory[id] = amount + add;
end

function RemoveItem(item, sub)
    local id;
    if (sub == nil) then
        sub = 1
    end

    if (type(item) == "string") then
        id = item
    else
        id = item.id
    end

    local amount = inventory[id]
    if (amount == nil) then
        return
    end
    inventory[id] = amount - sub;

    if (inventory[id] <= 0) then
        inventory[id] = nil;
    end
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
function GetInventoryStack(item)
    local id;

    if (type(item) == "string") then
        id = item
    else
        id = item.id
    end

    return inventory[id] or 0
end

-- ============================
-- Save inventory
-- ============================


SetDataLoader(function(data)
    if (data == nil) then
        return
    end
    inventory = data;
end)

SetDataSaver(function()
    -- run your logic here
    return inventory
end)


-- ============================
-- ITEM REGISTRY
-- ============================
itemRegistry = {}

function RegisterItem(item)
    itemRegistry[item.id] = item;
end

function GetItem(item)

    if (item == nil) then
        return nil;
    end

    if (type(item) == "string") then
        return itemRegistry[item];
    end

    return itemRegistry[item.id];
end

-- ============================
-- ITEM CLASS
-- ============================
Item = {}
---@param o {id: string}
function Item:new(o)
    if (o == nil) then
        error("nil properties passed to item constructor")
    end

    if (o.id == nil) then
        error("Property id not set")
    end
    self.__index = self

    o.data = o.data or {}
    o.tags = o.tags or {}

    setmetatable(o, self)
    return o
end

heldItem = nil
function Item:ShowOptions(renderer, context)
    --if no consumable ability, dont show actions
    if (self.consumableWorldAbility == nil) then
        renderer:AddText("inventory.item.noActions")

    elseif (self.canTargetPartyMember == true) then
        renderer:AddAction(function()
            heldItem = self
            context:ChangeState("select_party_member")
        end, "inventory.item.use")

    else
        renderer:AddAction(function()
            self.consumableWorldAbility(nil)
            RemoveItem(self)
            context:ChangeState("ui_inventory")
        end, "inventory.item.use")
    end
end

function GetHeldItem()
    return heldItem
end

function Item:DisplayName()
    return GetTranslation("item." .. self.id .. ".name")
end

function Item:GetApproximateStats()
    local stats = {}
    for i, v in ipairs(statsAbilityApproximationHandlers) do
        v(self, stats)
    end
    return stats
end

function Item:ParseAbility(abilityBuilder)
    if (self.abilities == nil) then
        return nil
    end
    local abilities = {}

    for key, value in pairs(self.abilities) do
        table.insert(abilities, abilityBuilder(key):Data(value):Build())
    end

    return abilities
end


-- ============================
-- INVENTORY UI
-- ============================
BlackListState("ui_inventory")
BlackListState("ui_item_details")

StateBuilder("ui_inventory")
        :Sticky(false)
        :Render(
        function(renderer, context)
            renderer:SetBackgroundColor("Brown")
            renderer:AddAction(function()
                context:ChangeState(OldState:Get())
            end, "inventory.close")
            renderer:AddText("inventory.header")
            for key, entry in ipairs(GetInventory()) do
                renderer:AddAction(function()
                    activeDetailItem = entry
                    context:ChangeState("ui_item_details")
                end, "inventory.item", { { "item", entry.item:DisplayName() }, { "amount", tostring(entry.amount) } })
            end
        end
)
        :Build()

StateBuilder("ui_item_details")
        :Sticky(false)
        :AllowSave(false)
        :Render(
        function(renderer, context)
            renderer:SetBackgroundColor("Brown")
            renderer:AddAction(function()
                context:ChangeState("ui_inventory")
            end, "inventory.item.close")
            renderer:AddText("inventory.item.header", { { "item", activeDetailItem.item:DisplayName() }, { "amount", tostring(activeDetailItem.amount) } })
            activeDetailItem.item:ShowOptions(renderer, context)
        end
)
        :Build()

Global:AddOnPostStateRender(
        function(renderer, context)
            if (IsUI(context.ActiveStateId)) then
                return
            end
            renderer:AddAction(function()
                context:ChangeState("ui_inventory")
            end, "inventory.open")
        end
)

CreateConsumableHandler(function()
    local consumables = {}

    for key, entry in ipairs(GetInventory()) do
        if (type(entry.item.consumableAbility) == "table") then
            for ability, data in pairs(entry.item.consumableAbility) do

                table.insert(consumables, {
                    amount = entry.amount,
                    name = entry.item:DisplayName(),
                    ability = ability,
                    abilityData = data,
                    onUse = function()
                        RemoveItem(entry.item)
                    end,
                })

            end
        end

    end

    return consumables
end)

-- ============================
-- EXPORTS
-- ============================
Context:CreateFunc("AddStatApproximationHandler", AddStatApproximationHandler)
Context:CreateFunc("AddItem", AddItem)
Context:CreateFunc("GetInventoryStack", GetInventoryStack)
Context:CreateFunc("GetAmount", GetInventoryStack) -- alternative name for GetInventoryStack
Context:CreateFunc("RemoveItem", RemoveItem)
Context:CreateFunc("GetInventory", GetInventory)
Context:CreateFunc("RegisterItem", RegisterItem)
Context:CreateFunc("GetItem", GetItem)
Context:CreateFunc("GetHeldItem", GetHeldItem)
Context:CreateVar("Item", Item)
