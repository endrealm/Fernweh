-- ============================
-- Imports
-- ============================

local compat = Import("ui_compat", "api")
BlackListState = compat:GetFunc("BlackListState")

local character = Import("character_system", "lib/character")
local party = Import("character_system", "lib/party")

local Character = character:Get("Character")
local AddToParty = party:GetFunc("AddToParty")
local IsInParty = party:GetFunc("IsInParty")
local SetPostBattleInfo = party:GetFunc("SetPostBattleInfo")

local inventory = Import("inventory", "api")
local AddItem = inventory:GetFunc("AddItem")

local quest_book = Import("quest_book", "api")
local AddQuest = quest_book:GetFunc("AddQuest")
local CloseQuest = quest_book:GetFunc("CloseQuest")
local IsQuestCompleted = quest_book:GetFunc("IsQuestCompleted")
local IsQuestDiscovered = quest_book:GetFunc("IsQuestDiscovered")
local HasJustCompletedQuest = quest_book:GetFunc("HasJustCompletedQuest")

local shop = Import("shops", "api")
local moneyHook = Import("shops", "money_hook")
AddMoney = moneyHook:GetFunc("AddMoney")
OpenShop = shop:GetFunc("OpenShop")

-- ============================
-- World Enter
-- ============================

BlackListState("enter_market")
StateBuilder("enter_market")
        :Render(
                function(renderer, context)
                    local exit = "enter_market"
                    if(IsQuestDiscovered("jewelery_quest") == false) then
                        exit = "jewelery_quest"
                    end

                    renderer:AddText("enter.market")
                    renderer:AddAction(function() OpenShop(context, {
                        exitState="enter_market",
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
                        }) end, "button.market.food")
                    renderer:AddAction(function() OpenShop(context, {
                        exitState=exit,
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
                        }) end, "button.market.jewelery")
                    if(IsQuestDiscovered("mugging_quest") == false) then
                        renderer:AddAction(function() context:ChangeState("mugging_quest") end, "button.market.crowd")
                    end
                    renderer:AddAction(function() context:Exit() end, "button.leave")
                end
        )
        :Build()

BlackListState("jewelery_quest")
StateBuilder("jewelery_quest")
        :Render(
                function(renderer, context)
                    renderer:AddText("jewelery.quest")
                    renderer:AddAction(function() 
                        AddQuest("jewelery_quest")
                        context:ChangeState("enter_market") 
                    end, "button.accept")
                    renderer:AddAction(function() context:ChangeState("enter_market") end, "button.decline")
                end
        )
        :Build()

BlackListState("mugging_quest")
StateBuilder("mugging_quest")
        :Render(
                function(renderer, context)
                    renderer:AddText("mugging.quest")
                    renderer:AddAction(function() 
                        AddQuest("mugging_quest")
                        context:ChangeState("enter_market") 
                    end, "button.accept")
                    renderer:AddAction(function() context:ChangeState("enter_market") end, "button.decline")
                end
        )
        :Build()

BlackListState("enter_inn")
StateBuilder("enter_inn")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.inn")
                    renderer:AddAction(function() context:Exit() end, "button.inn.sleep")
                    renderer:AddAction(function() context:ChangeState("enter_tavern") end, "button.inn.enter_tavern")
                    renderer:AddAction(function() context:Exit() end, "button.leave")
                end
        )
        :Build()

BlackListState("enter_tavern")
StateBuilder("enter_tavern")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.tavern")

                    if(IsInParty("Borof") == false) then
                        renderer:AddText("tavern.borof")
                    end

                    if(IsQuestCompleted("mugging_quest") == false) then
                        renderer:AddText("mugging.quest.battle")
                        renderer:AddAction(function() context:StartBattle({"bandit"}, "cave", "beat_mugger") end, "button.battle")
                    end

                    if(IsInParty("Borof") == false) then
                        renderer:AddAction(function() context:ChangeState("recruit_borok") end, "button.tavern.borof")
                    end
                    renderer:AddAction(function() context:ChangeState("enter_inn") end, "button.leave")
                end
        )
        :Build()

BlackListState("recruit_borok")
StateBuilder("recruit_borok")
        :Render(
                function(renderer, context)
                    renderer:AddText("tavern.borof.recruit")
                    renderer:AddAction(function()
                        AddToParty(Character:new({id = "Borof", stats = {health=23, mana=10, strength=17, intellect=12, dexterity=11, constitution=16, wisdom=12, charisma=8}, equip = {weapon="dagger", body="clothes", feet="shoes"}})) 
                        context:ChangeState("enter_tavern") 
                    end, "button.accept")
                    renderer:AddAction(function() context:ChangeState("enter_tavern") end, "button.decline")
                end
        )
        :Build()

BlackListState("beat_mugger")
StateBuilder("beat_mugger")
        :Render(
                function(renderer, context)
                    renderer:AddText("mugging.quest.complete")
                    CloseQuest("mugging_quest")
                    renderer:AddAction(function() context:ChangeState("enter_tavern") end, "button.leave")
                end
        )
        :Build()

BlackListState("enter_forge")
StateBuilder("enter_forge")
        :Render(
                function(renderer, context)
                    local exit = "enter_forge"
                    if(IsQuestDiscovered("forge_quest") == false) then
                        exit = "forge_quest"
                    end

                    renderer:AddText("enter.forge")
                    renderer:AddAction(function() OpenShop(context, {
                        exitState=exit,
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
                        }) end, "button.forge.weapons")
                    renderer:AddAction(function() OpenShop(context, {
                        exitState="enter_forge",
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
                        }) end, "button.forge.armor")
                    renderer:AddAction(function() context:Exit() end, "button.leave")
                end
        )
        :Build()

BlackListState("forge_quest")
StateBuilder("forge_quest")
        :Render(
                function(renderer, context)
                    renderer:AddText("forge.quest")
                    renderer:AddAction(function() 
                        AddQuest("forge_quest")
                        context:ChangeState("enter_forge") 
                    end, "button.accept")
                    renderer:AddAction(function() context:ChangeState("enter_forge") end, "button.decline")
                end
        )
        :Build()

BlackListState("enter_hall")
StateBuilder("enter_hall")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.hall")

                    if(HasJustCompletedQuest("tharmus_training") == true)
                    then
                        renderer:AddAction(function() context:ChangeState("dolrom_quest1") end, "button.checkquest.dolrom")
                    elseif(HasJustCompletedQuest("rat_cellar"))
                    then
                        renderer:AddAction(function() context:ChangeState("dolrom_quest2") end, "button.checkquest.dolrom")
                    end

                    renderer:AddAction(function() context:Exit() end, "button.leave")
                end
        )
        :Build()

-- ============================
-- World Leave
-- ============================

BlackListState("leave_market")
StateBuilder("leave_market")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.market")
                    renderer:AddText("", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_inn")
StateBuilder("leave_inn")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.inn")
                    renderer:AddText("", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_forge")
StateBuilder("leave_forge")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.forge")
                    renderer:AddText("", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_hall")
StateBuilder("leave_hall")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.hall")
                    renderer:AddText("", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_farcross")
StateBuilder("leave_farcross")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.farcross")
                    renderer:AddAction(function() 
                        context:LoadMap("testMap", 7, 10)
                        context:Exit() 
                    end, "button.leave")
                    renderer:AddAction(function() context:Exit() end, "button.reconsider")
                end
        )
        :Build()

-- ============================
-- Shops
-- ============================

BlackListState("market_food")
StateBuilder("market_food")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.hall")
                    renderer:AddAction(function() context:Exit() end, "button.leave")
                end
        )
        :Build()