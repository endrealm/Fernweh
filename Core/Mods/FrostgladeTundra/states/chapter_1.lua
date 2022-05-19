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

function Random(amount)
    return math.random(1, amount);
end

-- ============================
-- Temp testing
-- ============================
AddToParty(Character:new({id = "Gardain", stats = {health=13, mana=6, strength=17, intellect=13, dexterity=10, constitution=16, wisdom=12, charisma=12}, equip = {weapon="knife", body="clothes", feet="shoes"}}))
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
                AddToParty(Character:new({id = "Olma", stats = {health=13, mana=6, strength=15, intellect=9, dexterity=12, constitution=16, wisdom=11, charisma=14}, equip = {weapon="dagger", body="clothes", feet="fur_boots"}}))
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
                AddToParty(Character:new({id = "Luneiros", stats = {health=12, mana=2, strength=8, intellect=10, dexterity=17, constitution=14, wisdom=9, charisma=12}, equip = {weapon="shortsword", body="leather_armor", feet="fur_boots"}}))
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
                    if(Random(10) <= 4)
                    then
                        renderer:AddText("encounter", function() context:Exit() end)
                        renderer:AddAction(function() context:StartBattle({"polar_bear"}) end, "button.battle")
                        renderer:AddAction(function() context:Exit() end, "button.leave")
                    else
                        context:Exit()
                    end
                end
        )
        :Build()

BlackListState("enter_forest")
StateBuilder("enter_forest")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    if(Random(10) <= 6)
                    then
                        renderer:AddText("encounter", function() context:Exit() end)
                        renderer:AddAction(function() context:StartBattle({"polar_bear", "bandit"}) end, "button.battle")
                        renderer:AddAction(function() context:Exit() end, "button.leave")
                    else
                        context:Exit()
                    end
                end
        )
        :Build()

BlackListState("enter_path")
StateBuilder("enter_path")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    if(Random(10) <= 3)
                    then
                        renderer:AddText("encounter", function() context:Exit() end)
                        renderer:AddAction(function() context:StartBattle({"bandit", "bandit"}) end, "button.battle")
                        renderer:AddAction(function() context:Exit() end, "button.leave")

                    else
                        context:Exit()
                    end
                end
        )
        :Build()

BlackListState("enter_ice")
StateBuilder("enter_ice")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    if(Random(10) <= 5)
                    then
                        renderer:AddText("encounter", function() context:Exit() end)
                        renderer:AddAction(function() context:StartBattle({"polar_bear", "polar_bear"}) end, "button.battle") 
                        renderer:AddAction(function() context:Exit() end, "button.leave")
                    else
                        context:Exit()
                    end
                end
        )
        :Build()

-- ============================
-- World First Enter
-- ============================

BlackListState("first_enter_snow")
StateBuilder("first_enter_snow")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.snow", function() context:ChangeState("enter_snow") end)
                end
        )
        :Build()

BlackListState("first_enter_forest")
StateBuilder("first_enter_forest")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.forest", function() context:ChangeState("enter_forest") end)
                end
        )
        :Build()

BlackListState("first_enter_path")
StateBuilder("first_enter_path")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.path", function() context:ChangeState("enter_path") end)
                end
        )
        :Build()

BlackListState("first_enter_ice")
StateBuilder("first_enter_ice")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.ice", function() context:ChangeState("enter_ice") end)
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
                                itemId= "knife",
                            },
                            {
                                itemId= "dagger",
                            },
                            {
                                itemId= "shortsword",
                            },
                            {
                                itemId= "spear",
                            },
                            {
                                itemId= "wand",
                            },
                            {
                                itemId= "wooden_staff",
                            }
                            }
                        }) end, "button.enter.blacksmith")

                        renderer:AddAction(function() OpenShop(context, {
                        exitState="enter_castle",
                        offer= {
                            {
                                itemId= "toque",
                            },
                            {
                                itemId= "fur_hood",
                            },
                            {
                                itemId= "wizards_hat",
                            },
                            {
                                itemId= "clothes",
                            },
                            {
                                itemId= "wizards_robe",
                            },
                            {
                                itemId= "leather_armor",
                            },
                            {
                                itemId= "shoes",
                            },
                            {
                                itemId= "fur_boots",
                            },
                            {
                                itemId= "leather_boots",
                            }
                            }
                        }) end, "button.enter.armorer")

                        renderer:AddAction(function() OpenShop(context, {
                        exitState="enter_castle",
                        offer= {
                            {
                                itemId= "ruby_ring",
                            },
                            {
                                itemId= "silver_armlet",
                            },
                            {
                                itemId= "gold_necklace",
                            },
                            {
                                itemId= "pearl_earrings",
                            },
                            {
                                itemId= "book_charm",
                            },
                            {
                                itemId= "holy_talisman",
                            }
                            }
                        }) end, "button.enter.jeweler")

                        renderer:AddAction(function() context:Exit() end, "button.leave")
                    end
                end
        )
        :Build()

-- ============================
-- World Last Leave
-- ============================

BlackListState("last_leave_snow")
StateBuilder("last_leave_snow")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.snow", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("last_leave_forest")
StateBuilder("last_leave_forest")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.forest", function() context:Exit() end)
                end
        )
        :Build()


BlackListState("last_leave_path")
StateBuilder("last_leave_path")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.path", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("last_leave_ice")
StateBuilder("last_leave_ice")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.ice", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_castle")
StateBuilder("last_leave_castle")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.castle", function() context:Exit() end)
                end
        )
        :Build()