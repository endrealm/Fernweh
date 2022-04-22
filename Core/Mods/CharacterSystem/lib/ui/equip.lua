-- ============================
-- Imports
-- ============================
local party = Import("lib/party")
GetMembers = party:GetFunc("GetMembers")

local uiCompat = Import("ui_compat", "api")
BlackListState = uiCompat:GetFunc("BlackListState")
IsUI = uiCompat:GetFunc("IsUI")
OldState = uiCompat:GetVar("OldState")

-- ============================
-- Core
-- ============================
equipItem = nil
function SetEquipItem(item)
    equipItem = item;
end

-- ============================
-- UI
-- ============================
BlackListState("ui_equip_item")

StateBuilder("ui_equip_item")
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Brown")
                renderer:AddAction(function() context:ChangeState("ui_item_details") end, "inventory.close")
                renderer:AddText("equip.header")
                for i, character in ipairs(GetMembers()) do
                    renderer:AddAction(function()
                        
                    end, "equip.character", {{"name", character.id}})
                end
            end
        )
        :Build()
-- ============================
-- Exports
-- ============================
Context:CreateFunc("SetEquipItem", SetEquipItem)