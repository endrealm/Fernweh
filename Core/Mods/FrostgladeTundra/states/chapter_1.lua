-- ============================
-- Imports
-- ============================

local character = Import("character_system", "lib/character")
local party = Import("character_system", "lib/party")

local Character = character:Get("Character")
local AddToParty = party:GetFunc("AddToParty")

local shop = Import("shops", "api")
OpenShop = shop:GetFunc("OpenShop")

-- ============================
-- Temp testing
-- ============================
AddToParty(Character:new({id = "Yennifer", stats = {health=10, mana=5}}))
AddToParty(Character:new({id = "Triss"}))
AddToParty(Character:new({id = "Geralt"}))
AddToParty(Character:new({id = "Ciri"}))

Context:CreateStoredVar("test", "value")

SetEntryState("start_state")

StateBuilder("start_state")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.intro")
                renderer:AddAction(function() context:Exit() end, "button.accept")
                renderer:AddAction(function() OpenShop(context, {
                    exitState="start_state",
                    offer= {
                        {
                            itemId= "infinity_blade",
                            price=10,
                        },
                        {
                            itemId= "lethality_blade",
                            price=15,
                        }
                    }
                }) end, "test.shop")
            end
    )
    :Build()

StateBuilder("tharmus_training")
    :Render(
            function(renderer, context)
                renderer:AddText("tharmus.training.1")
                renderer:AddAction(function() context:StartBattle() end, "button.battle")
            end
    )
    :Build()