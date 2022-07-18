-- ============================
-- Imports
-- ============================

local compat = Import("ui_compat", "api")
BlackListState = compat:GetFunc("BlackListState")

local character = Import("character_system", "lib/character")
local party = Import("character_system", "lib/party")

local Character = character:Get("Character")
local AddToParty = party:GetFunc("AddToParty")
local SetPostBattleInfo = party:GetFunc("SetPostBattleInfo")

local inventory = Import("inventory", "api")
local AddItem = inventory:GetFunc("AddItem")

local quest_book = Import("quest_book", "api")
local AddQuest = quest_book:GetFunc("AddQuest")
local CloseQuest = quest_book:GetFunc("CloseQuest")
local IsQuestCompleted = quest_book:GetFunc("IsQuestCompleted")
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
                        exitState="enter_market",
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
                    renderer:AddAction(function() context:Exit() end, "button.market.crowd")
                    renderer:AddAction(function() context:Exit() end, "button.leave")
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
                    renderer:AddAction(function() context:ChangeState("enter_inn") end, "button.leave")
                end
        )
        :Build()

BlackListState("enter_forge")
StateBuilder("enter_forge")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.forge")
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
                        }) end, "button.forge.weapons")
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
                        }) end, "button.forge.armor")
                    renderer:AddAction(function() context:Exit() end, "button.leave")
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
                    renderer:AddText("leave.market", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_inn")
StateBuilder("leave_inn")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.inn", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_forge")
StateBuilder("leave_forge")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.forge", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_hall")
StateBuilder("leave_hall")
:ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.hall", function() context:Exit() end)
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