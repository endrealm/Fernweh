-- ============================
-- Imports
-- ============================

local quest_book = Import("quest_book", "api")
local Quest = quest_book:Get("Quest")

local RegisterQuest = quest_book:GetFunc("RegisterQuest")
local AddQuest = quest_book:GetFunc("AddQuest")

-- ============================
-- Quests
-- ============================

RegisterQuest(Quest:new({
    id = "tharmus_training",
    main = true,
    nextQuest = "rat_cellar",
}))

RegisterQuest(Quest:new({
    id = "rat_cellar",
    main = true,
    nextQuest = "kobold_camp",
}))

RegisterQuest(Quest:new({
    id = "kobold_camp",
    main = true,
}))