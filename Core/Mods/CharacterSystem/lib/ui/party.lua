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
-- UI States
-- ============================

BlackListState("ui_party")
BlackListState("ui_party_member_details")


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