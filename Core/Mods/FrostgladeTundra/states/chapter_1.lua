-- ============================
-- Imports
-- ============================

local compat = Import("ui_compat", "api")
BlackListState = compat:GetFunc("BlackListState")

local character = Import("character_system", "lib/character")
local party = Import("character_system", "lib/party")

local Character = character:Get("Character")
local AddToParty = party:GetFunc("AddToParty")

local inventory = Import("inventory", "api")
local AddItem = inventory:GetFunc("AddItem")

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

function StartEncounter(renderer, context, translation, enemies, chance)
	if(Random(100) <= chance)
    then
        renderer:AddText(translation)
        renderer:AddAction(function() context:StartBattle(enemies, "forest") end, "button.battle")
        renderer:AddAction(function() context:Exit() end, "button.battle.run")
    else
        context:Exit()
    end
end

-- ============================
-- Temp testing
-- ============================
AddToParty(Character:new({id = "Gardain", stats = {health=22, mana=10, strength=17, intellect=13, dexterity=10, constitution=16, wisdom=12, charisma=8}, equip = {weapon="knife", body="clothes", feet="shoes"}}))
-- AddToParty(Character:new({id = "Triss"}))
-- AddToParty(Character:new({id = "Geralt"}))
-- AddToParty(Character:new({id = "Ciri"}))

-- ============================
-- Quests
-- ============================

SetEntryState("start_state")

BlackListState("start_state")
StateBuilder("start_state")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.intro")
                AddItem("health_potion", 3)
                AddItem("revive", 1)
                AddItem("ether", 2)
                AddItem("key", 1)
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
                    renderer:AddAction(function() context:StartBattle({"guard", "guard"}, "forest", "tharmus_training_2") end, "button.battle")
                    renderer:AddAction(function() context:Exit() end, "button.decline")
                else
                    renderer:AddText("tharmus.training.4") -- if returning after beating tharmus
                    renderer:AddAction(function() context:StartBattle({"guard", "guard", "guard"}, "forest") end, "button.battle")
                    renderer:AddAction(function() context:Exit() end, "button.decline")
                end
            end
    )
    :Build()

StateBuilder("tharmus_training_2")
    :Render(
            function(renderer, context)
                renderer:AddText("tharmus.training.2")
                AddToParty(Character:new({id = "Olma", stats = {health=23, mana=16, strength=15, intellect=9, dexterity=14, constitution=17, wisdom=11, charisma=12}, equip = {weapon="wooden_staff", body="clothes", feet="fur_boots"}}))
                renderer:AddAction(function() context:StartBattle({"tharmus"}, "forest", "tharmus_training_finish") end, "button.battle")
            end
    )
    :Build()

StateBuilder("tharmus_training_finish")
    :Render(
            function(renderer, context)
                questProg:Set("1")
                renderer:AddText("tharmus.training.3", { { "reward", AddMoney(35) } })
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
                renderer:AddAction(function() context:StartBattle({"rat", "rat"}, "cave", "dolrom_quest1.2") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.2")
    :Render(
            function(renderer, context)
                AddItem("health_potion", 1)
                renderer:AddText("dolrom.quest.1.2")
                renderer:AddAction(function() context:StartBattle({"large_rat", "large_rat"}, "cave", "dolrom_quest1.3") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.3")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.1.3")
                renderer:AddAction(function() context:StartBattle({"rat", "rat", "rat", "rat"}, "cave", "dolrom_quest1.4") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.4")
    :Render(
            function(renderer, context)
                AddItem("health_potion", 1)
                renderer:AddText("dolrom.quest.1.4")
                renderer:AddAction(function() context:StartBattle({"large_rat", "large_rat", "rat", "rat", "rat"}, "cave", "dolrom_quest1.5") end, "button.battle")
            end
    )
    :Build()

StateBuilder("dolrom_quest1.5")
    :Render(
            function(renderer, context)
                questProg:Set("2")
                renderer:AddText("dolrom.quest.1.5", { { "reward", AddMoney(55) } })
                renderer:AddAction(function() context:ChangeState("enter_castle") end, "button.leave")
            end
    )
    :Build()

StateBuilder("dolrom_quest2")
    :Render(
            function(renderer, context)
                renderer:AddText("dolrom.quest.2")
                AddToParty(Character:new({id = "Luneiros", stats = {health=20, mana=12, strength=8, intellect=10, dexterity=17, constitution=14, wisdom=10, charisma=12}, equip = {weapon="halberd", body="leather_armor", feet="fur_boots"}}))
                renderer:AddAction(function() context:Exit() end, "button.accept")
            end
    )
    :Build()

StateBuilder("kobold_camp1")
    :Render(
            function(renderer, context)
                if(GetQuestProg() == "2")
                then
                    renderer:AddText("koboldcamp.quest.1")
                    renderer:AddAction(function() context:StartBattle({"gaint_weasel", "gaint_weasel"}, "forest", "kobold_camp1.1") end, "button.battle")
                elseif(GetQuestProg() == "0" or GetQuestProg() == "1")
                then
                    context:Exit()
                else
                    renderer:AddText("koboldcamp.quest.1.6")
                    renderer:AddAction(function() context:Exit() end, "button.leave")
                end
            end
    )
    :Build()

StateBuilder("game_conclusion")
    :Render(
            function(renderer, context)
                renderer:AddText("game.conclusion")
                renderer:AddAction(function() context:Exit() end, "button.accept")
            end
    )
    :Build()

StateBuilder("kobold_camp1.1")
    :Render(
            function(renderer, context)
                renderer:AddText("koboldcamp.quest.1.1")
                renderer:AddAction(function() context:StartBattle({"kobold_melee", "kobold_melee"}, "cave", "kobold_camp1.2") end, "button.battle")
            end
    )
    :Build()

StateBuilder("kobold_camp1.2")
    :Render(
            function(renderer, context)
                renderer:AddText("koboldcamp.quest.1.2")
                renderer:AddAction(function() context:StartBattle({"kobold_melee", "kobold_ranged", "kobold_ranged"}, "cave", "kobold_camp1.3") end, "button.battle")
            end
    )
    :Build()

StateBuilder("kobold_camp1.3")
    :Render(
            function(renderer, context)
                renderer:AddText("koboldcamp.quest.1.3")
                renderer:AddAction(function() context:StartBattle({"wolf", "wolf"}, "forest", "kobold_camp1.4") end, "button.battle")
            end
    )
    :Build()

StateBuilder("kobold_camp1.4")
    :Render(
            function(renderer, context)
                renderer:AddText("koboldcamp.quest.1.4")
                renderer:AddAction(function() context:StartBattle({"kobold_ranged", "kobold_melee", "kobold_ranged", "gaint_weasel", "gaint_weasel", "wolf"}, "cave", "kobold_camp1.5") end, "button.battle")
            end
    )
    :Build()

StateBuilder("kobold_camp1.5")
    :Render(
            function(renderer, context)
                questProg:Set("3")
                AddItem("halberd", 1)
                renderer:AddText("koboldcamp.quest.1.5", { { "reward", AddMoney(50) } })
                renderer:AddAction(function() context:Exit() end, "button.leave")
            end
    )
    :Build()

-- ============================
-- World Enter
-- ============================

BlackListState("enter_snow")
StateBuilder("enter_snow")
        :Render(
                function(renderer, context)
                    StartEncounter(renderer, context, "encounter.wolf", {"wolf"}, 40)
                end
        )
        :Build()

BlackListState("enter_forest")
StateBuilder("enter_forest")
        :Render(
                function(renderer, context)
                    StartEncounter(renderer, context, "encounter.wolf", {"wolf", "bandit"}, 50)
                end
        )
        :Build()

BlackListState("enter_path")
StateBuilder("enter_path")
        :Render(
                function(renderer, context)
                    StartEncounter(renderer, context, "encounter.bandit", {"bandit", "bandit"}, 20)
                end
        )
        :Build()

BlackListState("enter_ice")
StateBuilder("enter_ice")
        :Render(
                function(renderer, context)
                    StartEncounter(renderer, context, "encounter.wolf", {"wolf", "wolf"}, 30)
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
                    renderer:AddText("enter.snow")
                    StartEncounter(renderer, context, "encounter.wolf", {"wolf"}, 40)
                end
        )
        :Build()

BlackListState("first_enter_forest")
StateBuilder("first_enter_forest")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.forest")
                    StartEncounter(renderer, context, "encounter.wolf", {"wolf", "bandit"}, 50)
                end
        )
        :Build()

BlackListState("first_enter_path")
StateBuilder("first_enter_path")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.path")
                    StartEncounter(renderer, context, "encounter.bandit", {"bandit", "bandit"}, 20)
                end
        )
        :Build()

BlackListState("first_enter_ice")
StateBuilder("first_enter_ice")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.ice")
                    StartEncounter(renderer, context, "encounter.wolf", {"wolf", "wolf"}, 30)
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
                                itemId= "longsword",
                            },
                            {
                                itemId= "infinity_blade",
                            },
                            {
                                itemId= "lethality_blade",
                            },
                            {
                                itemId= "spear",
                            },
                            {
                                itemId= "halberd",
                            },
                            {
                                itemId= "wand",
                            },
                            {
                                itemId= "wooden_staff",
                            },
                            {
                                itemId= "sages_staff",
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
                                itemId= "leather_cap",
                            },
                            {
                                itemId= "iron_helm",
                            },
                            {
                                itemId= "clothes",
                            },
                            {
                                itemId= "wizards_robe",
                            },
                            {
                                itemId= "sages_robe",
                            },
                            {
                                itemId= "leather_armor",
                            },
                            {
                                itemId= "scaled_armor",
                            },
                            {
                                itemId= "iron_armor",
                            },
                            {
                                itemId= "shoes",
                            },
                            {
                                itemId= "fur_boots",
                            },
                            {
                                itemId= "leather_boots",
                            },
                            {
                                itemId= "plated_boots",
                            },
                            {
                                itemId= "iron_boots",
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

                        renderer:AddAction(function() OpenShop(context, {
                        exitState="enter_castle",
                        offer= {
                            {
                                itemId= "health_potion",
                            },
                            {
                                itemId= "ether",
                            },
                            {
                                itemId= "revive",
                            }
                            }
                        }) end, "button.enter.shop")

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
StateBuilder("leave_castle")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.castle", function() context:Exit() end)
                end
        )
        :Build()