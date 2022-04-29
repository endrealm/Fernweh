-- ============================
-- Imports
-- ============================
local party = Import("lib/party")
GetMembers = party:GetFunc("GetMembers")

local inventory = Import("inventory", "api")
AddItemToInventory = inventory:GetFunc("AddItem")
RemoveItemFromInventory = inventory:GetFunc("RemoveItem")

local uiCompat = Import("ui_compat", "api")
BlackListState = uiCompat:GetFunc("BlackListState")
IsUI = uiCompat:GetFunc("IsUI")
OldState = uiCompat:GetVar("OldState")

-- ============================
-- Core
-- ============================
equipItem = nil
equipChar = nil
function SetEquipItem(item)
    equipItem = item;
end

-- ============================
-- UI
-- ============================
BlackListState("ui_equip_item")
BlackListState("ui_equip_item_replace")

StateBuilder("ui_equip_item")
        :AllowSave(false)
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Brown")
                renderer:AddAction(function() context:ChangeState("ui_item_details") end, "equip.close")
                renderer:AddText("equip.header")
                for i, character in ipairs(GetMembers()) do
                    local currentItem = character:GetEquippedItem(equipItem.slot)
                    local currentItemName = "empty"
                    if(currentItem ~= nil) then
                        currentItemName = currentItem.id
                    end
                    
                    renderer:AddAction(function()
                        equipChar = character
                        context:ChangeState("ui_equip_item_replace")
                    end, "equip.character", {{"name", character.id}, {"item", currentItemName}})
                end
            end
        )
        :Build()

StateBuilder("ui_equip_item_replace")
        :AllowSave(false)
        :Sticky(false)
        :Render(
        function(renderer, context)
            renderer:SetBackgroundColor("Brown")
            local currentItem = equipChar:GetEquippedItem(equipItem.slot)

            if(currentItem ~= nil) then
                renderer:AddText("equip.replace.header.replace", {{"item", currentItem.id}})
            else
                renderer:AddText("equip.replace.header.noReplace")
            end

            renderer:AddAction(function()
                equipChar:EquipItem(equipItem, equipItem.slot)
                RemoveItemFromInventory(equipItem)
                if(currentItem ~= nil) then
                    AddItemToInventory(currentItem)
                end
                
                context:ChangeState("ui_inventory")
            end, "equip.replace.yes")
            renderer:AddAction(function() context:ChangeState("ui_equip_item") end, "equip.replace.no")

        end
)
        :Build()
-- ============================
-- Exports
-- ============================
Context:CreateFunc("SetEquipItem", SetEquipItem)