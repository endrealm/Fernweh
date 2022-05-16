-- ============================
-- Imports
-- ============================

local compat = Import("ui_compat", "api")
BlackListState = compat:GetFunc("BlackListState")

local character = Import("character_system", "lib/character")
local party = Import("character_system", "lib/party")

local Character = character:Get("Character")
local AddToParty = party:GetFunc("AddToParty")

local shop = Import("shops", "api")
local moneyHook = Import("shops", "money_hook")
AddMoney = moneyHook:GetFunc("AddMoney")
OpenShop = shop:GetFunc("OpenShop")

local questProg = Context:CreateStoredVar("questProg", "0")
function GetQuestProg()
    return questProg:Get();
end

-- ============================
-- Temp testing
-- ============================
AddToParty(Character:new({id = "Gardain", stats = {health=13, mana=6, strength=17, intellect=13, dexterity=10, constitution=16, wisdom=12, charisma=12}}))
-- AddToParty(Character:new({id = "Triss"}))
-- AddToParty(Character:new({id = "Geralt"}))
-- AddToParty(Character:new({id = "Ciri"}))

-- ============================
-- Quests
-- ============================

SetEntryState("start_state")

StateBuilder("start_state")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.intro")
                renderer:AddAction(function() context:Exit() end, "button.accept")
            end
    )
    :Build()

StateBuilder("tharmus_training")
    :Render(
            function(renderer, context)
                if(GetQuestProg() == "0")
                then
                    renderer:AddText("tharmus.training.1") -- if first time 
                    renderer:AddAction(function() context:StartBattle({"guard", "guard"}, "tharmus_training_2") end, "button.battle")
                    renderer:AddAction(function() context:Exit() end, "button.decline")
                else
                    renderer:AddText("tharmus.training.4") -- if returning after beating tharmus
                    renderer:AddAction(function() context:StartBattle({"guard", "guard", "guard"}) end, "button.battle")
                    renderer:AddAction(function() context:Exit() end, "button.decline")
                end
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
                questProg:Set("1")
                renderer:AddText("tharmus.training.3", { { "reward", AddMoney(15) } })
                renderer:AddAction(function() context:Exit() end, "button.accept")
            end
    )
    :Build()

StateBuilder("dolrom_quest1")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.1")
                renderer:AddAction(function() context:ChangeState("dolrom_quest1.1") end, "button.accept")
                renderer:AddAction(function() context:ChangeState("enter_castle") end, "button.decline")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.1")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.1.1")
                renderer:AddAction(function() context:StartBattle({"rat", "rat", "rat"}, "dolrom_quest1.2") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.2")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.1.2")
                renderer:AddAction(function() context:StartBattle({"large_rat", "large_rat"}, "dolrom_quest1.3") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.3")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.1.3")
                renderer:AddAction(function() context:StartBattle({"rat", "rat", "rat", "rat", "rat"}, "dolrom_quest1.4") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.4")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.1.4")
                renderer:AddAction(function() context:StartBattle({"large_rat", "large_rat", "large_rat"}, "dolrom_quest1.5") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.5")
    :Render(
            function(renderer, context)
                questProg:Set("2")
                renderer:AddText("dolrom.quest.1.5", { { "reward", AddMoney(25) } })
                renderer:AddAction(function() context:ChangeState("enter_castle") end, "button.leave")
            end
    )
    :Build()

StateBuilder("dolrom_quest2")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.2")
                AddToParty(Character:new({id = "Luneiros", stats = {health=12, mana=2, strength=8, intellect=10, dexterity=17, constitution=14, wisdom=9, charisma=12}}))
                renderer:AddAction(function() context:Exit() end, "button.accept")
            end
    )
    :Build()

-- ============================
-- World Enter
-- ============================

BlackListState("enter_snow")
StateBuilder("enter_snow")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.snow", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("enter_forest")
StateBuilder("enter_forest")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.forest", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("enter_path")
StateBuilder("enter_path")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.path", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("enter_ice")
StateBuilder("enter_ice")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.ice", function() context:Exit() end)
                end
        )
        :Build()

StateBuilder("enter_castle")
        :Render(
                function(renderer, context)
                    if(GetQuestProg() == "0")
                    then
                        renderer:AddText("dolrom.quest.intro.direct")
                        renderer:AddAction(function() context:Exit() end, "button.leave")
                    else

                        renderer:AddText("enter.castle")
                    
                        if(GetQuestProg() == "1")
                        then
                            renderer:AddAction(function() context:ChangeState("dolrom_quest1") end, "button.checkquest.dolrom")
                        elseif(GetQuestProg() == "2")
                        then
                            renderer:AddAction(function() context:ChangeState("dolrom_quest2") end, "button.checkquest.dolrom")
                        end

                        renderer:AddAction(function() OpenShop(context, {
                        exitState="enter_castle",
                        offer= {
                            {
                                itemId= "infinity_blade",
                                price=10,
                            },
                            {
                                itemId= "lethality_blade",
                            }
                            }
                        }) end, "button.enter.shop")

                        renderer:AddAction(function() context:Exit() end, "button.leave")
                    end
                end
        )
        :Build()

-- ============================
-- World Leave
-- ============================

        BlackListState("leave_snow")
StateBuilder("leave_snow")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.snow", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_forest")
StateBuilder("leave_forest")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.forest", function() context:Exit() end)
                end
        )
        :Build()


BlackListState("leave_path")
StateBuilder("leave_path")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.path", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_ice")
StateBuilder("leave_ice")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.ice", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_castle")
StateBuilder("leave_castle")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.castle", function() context:Exit() end)
                end
        )
        :Build()