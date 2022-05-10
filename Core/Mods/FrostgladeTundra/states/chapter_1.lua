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
AddToParty(Character:new({id = "Gardain", stats = {health=13, mana=6, strength=17, intellect=13, dexterity=10, constitution=16, wisdom=12, charisma=12}}))
-- AddToParty(Character:new({id = "Triss"}))
-- AddToParty(Character:new({id = "Geralt"}))
-- AddToParty(Character:new({id = "Ciri"}))

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
                renderer:AddAction(function() context:StartBattle({"guard", "guard"}, "tharmus_training_2") end, "button.battle")
                renderer:AddAction(function() context:Exit() end, "button.decline")
            end
    )
    :Build()

StateBuilder("tharmus_training_2")
    :Render(
            function(renderer, context)
                renderer:AddText("tharmus.training.2")
                AddToParty(Character:new({id = "Olma", stats = {health=13, mana=6, strength=15, intellect=9, dexterity=12, constitution=16, wisdom=11, charisma=14}}))
                renderer:AddAction(function() context:StartBattle({"tharmus"}, "tharmus_training_finish") end, "button.battle")
            end
    )
    :Build()

StateBuilder("tharmus_training_finish")
    :Render(
            function(renderer, context)
                renderer:AddText("tharmus.training.3")
                renderer:AddAction(function() context:Exit() end, "button.accept")
            end
    )
    :Build()