-- ============================
-- Imports
-- ============================
local party = Import("lib/party")
GetMembers = party:GetFunc("GetMembers")
local uiCompat = Import("ui_compat", "api")
BlackListState = uiCompat:GetFunc("BlackListState")
IsUI = uiCompat:GetFunc("IsUI")
OldState = uiCompat:GetVar("OldState")
local equip = Import("lib/equip")
GetSlots = equip:GetFunc("GetSlots")

local inventory = Import("inventory", "api")
AddItemToInventory = inventory:GetFunc("AddItem")

-- ============================
-- UI States
-- ============================

BlackListState("ui_party")
BlackListState("ui_party_member_details")
BlackListState("ui_party_member_details_slot")


StateBuilder("ui_party")
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Purple")
                renderer:AddAction(function() context:ChangeState(OldState:Get()) end, "party.close")
                renderer:AddText("party.header")
                for key, character in ipairs(GetMembers()) do
                    renderer:AddAction(function()
                        activeDetailMember=character
                        context:ChangeState("ui_party_member_details")
                    end, "party.list.member", { { "name", character.id } } )
                end
            end
        )
        :Build()

StateBuilder("ui_party_member_details")
        :AllowSave(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Purple")
                renderer:AddAction(function() context:ChangeState("ui_party") end, "party.back")
                renderer:AddText("party.detail.header", { { "name", activeDetailMember.id } })
                for key, value in pairs(activeDetailMember.stats) do
                    renderer:AddText("party.detail.stat."..key, { { "value", value } })
                end
                renderer:AddText("party.detail.items")
                for _, value in pairs(GetSlots()) do
                    local item = activeDetailMember:GetEquippedItem(value)
                    if(item == nil) then
                        item = "none"
                    else
                        item = item:DisplayName()
                    end
                    renderer:AddAction(function()
                        activeSlot = value
                        context:ChangeState("ui_party_member_details_slot") 
                    end, "party.detail.slot", { { "slot", GetTranslation("slot."..value..".name") }, { "item", item } })
                end
            end
        )
        :Build()

StateBuilder("ui_party_member_details_slot")
        :AllowSave(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("Purple")
                renderer:AddAction(function() context:ChangeState("ui_party_member_details") end, "party.back")
                renderer:AddText("party.detail.slot.header", { { "slot", GetTranslation("slot."..activeSlot..".name") } })
                local item = activeDetailMember:GetEquippedItem(activeSlot)
                local itemName = "none"
                if(item ~= nil) then
                    itemName = item:DisplayName()
                end
                renderer:AddText("party.detail.slot.current", { { "item", itemName } })
                
                if(item ~= nil) then
                    renderer:AddAction(function()
                        activeDetailMember:UnequipItem(activeSlot)
                        if(currentItem ~= nil) then
                            AddItemToInventory(item)
                        end
                        context:ChangeState("ui_party_member_details")
                    end, "party.details.unequip")
                end 
            end
        )
        :Build()


-- ============================
-- Event Listeners
-- ============================

Global:AddOnPostStateRender(
    function(renderer, context)
        if(IsUI(context.ActiveStateId)) then
            return
        end
        renderer:AddAction(function() context:ChangeState("ui_party") end, "party.open")
    end
)