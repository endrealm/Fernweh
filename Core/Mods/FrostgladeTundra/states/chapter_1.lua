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

function Random(amount)
    return math.random(1, amount);
end

function StartEncounter(renderer, context, translation, enemies, chance)
    if (Random(100) <= chance)
    then
        renderer:AddText(translation)
        renderer:AddAction(function()
            SetPostBattleInfo(100, { "mushroom" })
            context:StartBattle(enemies, "forest")
        end, "button.battle")
        renderer:AddAction(function()
            context:Exit()
        end, "button.battle.run")
    else
        context:Exit()
    end
end

-- ============================
-- Temp testing
-- ============================
AddToParty(Character:new({ id = "Gardain", stats = { health = 22, mana = 10, strength = 17, intellect = 13, dexterity = 10, constitution = 16, wisdom = 12, charisma = 8 }, equip = { weapon = "knife", body = "clothes", feet = "shoes" } }))
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
            context:LoadMap("testMap", 7, 10)
            renderer:AddText("dolrom.quest.intro")
            AddQuest("tharmus_training")
            AddItem("health_potion", 3)
            AddItem("revive", 1)
            AddItem("ether", 2)
            AddItem("key", 1)
            renderer:AddAction(function()
                context:Exit()
            end, "button.accept")
        end
)
        :Build()

StateBuilder("tharmus_training")
        :Render(
        function(renderer, context)
            if (IsQuestCompleted("tharmus_training") == false)
            then
                renderer:AddText("tharmus.training.1") -- if first time 
                renderer:AddAction(function()
                    context:StartBattle({ "guard", "guard" }, "forest", "tharmus_training_2")
                end, "button.battle")
                renderer:AddAction(function()
                    context:Exit()
                end, "button.decline")
            else
                renderer:AddText("tharmus.training.4") -- if returning after beating tharmus
                renderer:AddAction(function()
                    context:StartBattle({ "guard", "guard", "guard" }, "forest")
                end, "button.battle")
                renderer:AddAction(function()
                    context:Exit()
                end, "button.decline")
            end
        end
)
        :Build()

StateBuilder("tharmus_training_2")
        :Render(
        function(renderer, context)
            renderer:AddText("tharmus.training.2")
            AddToParty(Character:new({ id = "Olma", stats = { health = 23, mana = 16, strength = 15, intellect = 9, dexterity = 14, constitution = 17, wisdom = 11, charisma = 12 }, equip = { weapon = "wooden_staff", body = "clothes", feet = "fur_boots" } }))
            renderer:AddAction(function()
                context:StartBattle({ "tharmus" }, "forest", "tharmus_training_finish")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("tharmus_training_finish")
        :Render(
        function(renderer, context)
            CloseQuest("tharmus_training")
            renderer:AddText("tharmus.training.3", { { "reward", AddMoney(50) } })
            renderer:AddAction(function()
                context:Exit()
            end, "button.accept")
        end
)
        :Build()

StateBuilder("dolrom_quest1")
        :Render(
        function(renderer, context)
            renderer:AddText("dolrom.quest.1")
            renderer:AddAction(function()
                context:ChangeState("dolrom_quest1.1")
            end, "button.accept")
            renderer:AddAction(function()
                context:ChangeState("enter_castle")
            end, "button.decline")
        end
)
        :Build()

StateBuilder("dolrom_quest1.1")
        :Render(
        function(renderer, context)
            AddQuest("rat_cellar")
            renderer:AddText("dolrom.quest.1.1")
            renderer:AddAction(function()
                context:StartBattle({ "rat", "rat" }, "cave", "dolrom_quest1.2")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("dolrom_quest1.2")
        :Render(
        function(renderer, context)
            AddItem("health_potion", 1)
            renderer:AddText("dolrom.quest.1.2")
            renderer:AddAction(function()
                context:StartBattle({ "large_rat", "large_rat" }, "cave", "dolrom_quest1.3")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("dolrom_quest1.3")
        :Render(
        function(renderer, context)
            renderer:AddText("dolrom.quest.1.3")
            renderer:AddAction(function()
                context:StartBattle({ "rat", "rat", "rat", "rat" }, "cave", "dolrom_quest1.4")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("dolrom_quest1.4")
        :Render(
        function(renderer, context)
            AddItem("health_potion", 1)
            renderer:AddText("dolrom.quest.1.4")
            renderer:AddAction(function()
                context:StartBattle({ "large_rat", "large_rat", "rat", "rat", "rat" }, "cave", "dolrom_quest1.5")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("dolrom_quest1.5")
        :Render(
        function(renderer, context)
            CloseQuest("rat_cellar")
            renderer:AddText("dolrom.quest.1.5", { { "reward", AddMoney(75) } })
            renderer:AddAction(function()
                context:ChangeState("enter_castle")
            end, "button.leave")
        end
)
        :Build()

StateBuilder("dolrom_quest2")
        :Render(
        function(renderer, context)
            renderer:AddText("dolrom.quest.2")
            AddQuest("kobold_camp")
            AddToParty(Character:new({ id = "Luneiros", stats = { health = 20, mana = 12, strength = 8, intellect = 10, dexterity = 17, constitution = 14, wisdom = 10, charisma = 12 }, equip = { weapon = "halberd", body = "leather_armor", feet = "fur_boots" } }))
            renderer:AddAction(function()
                context:Exit()
            end, "button.accept")
        end
)
        :Build()

StateBuilder("kobold_camp1")
        :Render(
        function(renderer, context)
            if (HasJustCompletedQuest("rat_cellar"))
            then
                renderer:AddText("koboldcamp.quest.1")
                renderer:AddAction(function()
                    context:StartBattle({ "gaint_weasel", "gaint_weasel" }, "forest", "kobold_camp1.1")
                end, "button.battle")
            elseif (IsQuestCompleted("kobold_camp"))
            then
                renderer:AddText("koboldcamp.quest.1.6")
                renderer:AddAction(function()
                    context:Exit()
                end, "button.leave")
            else
                context:Exit()
            end
        end
)
        :Build()

StateBuilder("kobold_camp1.1")
        :Render(
        function(renderer, context)
            renderer:AddText("koboldcamp.quest.1.1")
            renderer:AddAction(function()
                context:StartBattle({ "kobold_melee", "kobold_melee" }, "cave", "kobold_camp1.2")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("kobold_camp1.2")
        :Render(
        function(renderer, context)
            renderer:AddText("koboldcamp.quest.1.2")
            renderer:AddAction(function()
                context:StartBattle({ "kobold_melee", "kobold_ranged", "kobold_ranged" }, "cave", "kobold_camp1.3")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("kobold_camp1.3")
        :Render(
        function(renderer, context)
            renderer:AddText("koboldcamp.quest.1.3")
            renderer:AddAction(function()
                context:StartBattle({ "wolf", "wolf" }, "forest", "kobold_camp1.4")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("kobold_camp1.4")
        :Render(
        function(renderer, context)
            renderer:AddText("koboldcamp.quest.1.4")
            renderer:AddAction(function()
                context:StartBattle({ "kobold_ranged", "kobold_melee", "kobold_ranged", "gaint_weasel", "gaint_weasel", "wolf" }, "cave", "kobold_camp1.5")
            end, "button.battle")
        end
)
        :Build()

StateBuilder("kobold_camp1.5")
        :Render(
        function(renderer, context)
            AddItem("halberd", 1)
            CloseQuest("kobold_camp")
            renderer:AddText("koboldcamp.quest.1.5", { { "reward", AddMoney(100) } })
            renderer:AddAction(function()
                context:Exit()
            end, "button.leave")
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
            StartEncounter(renderer, context, "encounter.wolf", { "wolf" }, 40)
        end
)
        :Build()

BlackListState("enter_forest")
StateBuilder("enter_forest")
        :Render(
        function(renderer, context)
            StartEncounter(renderer, context, "encounter.wolf", { "wolf", "bandit" }, 50)
        end
)
        :Build()

BlackListState("enter_path")
StateBuilder("enter_path")
        :Render(
        function(renderer, context)
            StartEncounter(renderer, context, "encounter.bandit", { "bandit", "bandit" }, 20)
        end
)
        :Build()

BlackListState("enter_ice")
StateBuilder("enter_ice")
        :Render(
        function(renderer, context)
            StartEncounter(renderer, context, "encounter.wolf", { "wolf", "wolf" }, 30)
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
            StartEncounter(renderer, context, "encounter.wolf", { "wolf" }, 40)
        end
)
        :Build()

BlackListState("first_enter_forest")
StateBuilder("first_enter_forest")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("enter.forest")
            StartEncounter(renderer, context, "encounter.wolf", { "wolf", "bandit" }, 50)
        end
)
        :Build()

BlackListState("first_enter_path")
StateBuilder("first_enter_path")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("enter.path")
            StartEncounter(renderer, context, "encounter.bandit", { "bandit", "bandit" }, 20)
        end
)
        :Build()

BlackListState("first_enter_ice")
StateBuilder("first_enter_ice")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("enter.ice")
            StartEncounter(renderer, context, "encounter.wolf", { "wolf", "wolf" }, 30)
        end
)
        :Build()

StateBuilder("enter_mine")
        :Render(
        function(renderer, context)
            renderer:AddText("enter.mine")
            renderer:AddAction(function()
                context:LoadMap("mineTest", 0, 2)
                context:Exit()
            end, "button.enter")
        end
)
        :Build()

StateBuilder("enter_castle")
        :Render(
        function(renderer, context)
            if (IsQuestCompleted("tharmus_training") == false)
            then
                renderer:AddText("dolrom.quest.intro.direct")
                renderer:AddAction(function()
                    context:Exit()
                end, "button.leave")
            else

                context:LoadMap("farcross", 2, 3)
                context:Exit()
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
            renderer:AddText("leave.snow", function()
                context:Exit()
            end)
        end
)
        :Build()

BlackListState("last_leave_forest")
StateBuilder("last_leave_forest")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("leave.forest", function()
                context:Exit()
            end)
        end
)
        :Build()

BlackListState("last_leave_path")
StateBuilder("last_leave_path")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("leave.path", function()
                context:Exit()
            end)
        end
)
        :Build()

BlackListState("last_leave_ice")
StateBuilder("last_leave_ice")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("leave.ice", function()
                context:Exit()
            end)
        end
)
        :Build()

BlackListState("leave_castle")
StateBuilder("leave_castle")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("leave.castle", function()
                context:Exit()
            end)
        end
)
        :Build()

BlackListState("leave_mine")
StateBuilder("leave_mine")
        :ClearScreenPost(false)
        :Render(
        function(renderer, context)
            renderer:AddText("leave.mine", function()
                context:Exit()
            end)
        end
)
        :Build()