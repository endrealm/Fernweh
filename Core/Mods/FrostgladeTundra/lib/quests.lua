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
}))