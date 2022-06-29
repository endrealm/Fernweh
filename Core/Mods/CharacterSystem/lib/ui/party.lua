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
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("DarkSlateBlue")
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
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("DarkSlateBlue")
                renderer:AddAction(function() context:ChangeState("ui_party") end, "party.back")
                renderer:AddText("party.detail.header", { { "name", activeDetailMember.id } })
                local bonusStats = activeDetailMember:GetItemStats()
                --for key, value in pairs(activeDetailMember.stats) do -- replaced this because this shows random order
                --    renderer:AddText("party.detail.stat."..key, { { "value", value }, { "bonus", bonusStats[key] or 0 } })
                --end

                renderer:AddText("party.detail.stat.experience", {{ "exp", activeDetailMember.stats.experience }, { "levelExp", activeDetailMember:GetExperienceForLevelUp() }, { "level", activeDetailMember.stats.level } })

                renderer:AddText("party.detail.stat.health_mana", { { "currentHP", activeDetailMember.current.health }, { "HP", activeDetailMember.stats.health }, { "bonusHP", bonusStats["health"] or 0 },
                { "currentMP", activeDetailMember.current.mana }, { "MP", activeDetailMember.stats.mana }, { "bonusMP", bonusStats["mana"] or 0 }})

                renderer:AddText("party.detail.stat.armor", {{ "value", activeDetailMember.stats.armor }, { "bonus", bonusStats["armor"] or 0 } })
                renderer:AddText("party.detail.stat.strength", {{ "value", activeDetailMember.stats.strength }, { "bonus", bonusStats["strength"] or 0 } })
                renderer:AddText("party.detail.stat.constitution", {{ "value", activeDetailMember.stats.constitution }, { "bonus", bonusStats["constitution"] or 0 } })
                renderer:AddText("party.detail.stat.dexterity", {{ "value", activeDetailMember.stats.dexterity }, { "bonus", bonusStats["dexterity"] or 0 } })
                renderer:AddText("party.detail.stat.intellect", {{ "value", activeDetailMember.stats.intellect }, { "bonus", bonusStats["intellect"] or 0 } })
                renderer:AddText("party.detail.stat.wisdom", {{ "value", activeDetailMember.stats.wisdom }, { "bonus", bonusStats["wisdom"] or 0 } })
                renderer:AddText("party.detail.stat.charisma", {{ "value", activeDetailMember.stats.charisma }, { "bonus", bonusStats["charisma"] or 0 } })

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
                renderer:SetBackgroundColor("DarkSlateBlue")
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