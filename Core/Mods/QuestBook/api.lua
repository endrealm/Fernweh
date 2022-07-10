-- ============================
-- Imports
-- ============================
local uiCompat = Import("ui_compat", "api")
BlackListState = uiCompat:GetFunc("BlackListState")
IsUI = uiCompat:GetFunc("IsUI")
OldState = uiCompat:GetVar("OldState")

-- ============================
-- Quest Book
-- ============================
quest_book = {}

function AddQuest(quest)
    local id;

    if(type(quest) == "string") then
        id = quest
    else
        id = quest.id
    end
    
    quest_book[id] = false;
end

function CloseQuest(quest)
    local id;

    if(type(quest) == "string") then
        id = quest
    else
        id = quest.id
    end

    quest_book[id] = true;
end

function RemoveQuest(quest)
    local id;

    if(type(quest) == "string") then
        id = quest
    else
        id = quest.id
    end

    quest_book[id] = nil;
end

function IsQuestCompleted(id)
    return quest_book[id]
end

function HasJustCompletedQuest(id)
    if(quest_book[id] == true) 
    then
        if(questRegistry[id].nextQuest == nil or quest_book[questRegistry[id].nextQuest] == nil or quest_book[questRegistry[id].nextQuest] == false) then
            return true
        end
    end
    return false
end

function GetQuestBook()
    local quests = {}

    for key, value in pairs(quest_book) do
        table.insert(quests, {
            quest = questRegistry[key],
            completed = value,
        })
    end
    
    return quests
end

-- ============================
-- QUEST REGISTRY
-- ============================
questRegistry = {}

function RegisterQuest(quest)
    questRegistry[quest.id] = quest;
end

function GetQuest(quest)
    
    if(quest == nil) then
        return nil;
    end

    if(type(quest) == "string") then
        return questRegistry[quest];
    end
    
    return questRegistry[quest.id];
end

-- ============================
-- Save Quest Book
-- ============================


SetDataLoader(function(data)
    if(data == nil) then
        return
    end
    quest_book = data;
end)

SetDataSaver(function()
    return quest_book
end)

-- ============================
-- QUEST CLASS
-- ============================
Quest = {}
---@param o {id: string}
function Quest:new(o)
    if(o == nil) then
        error("nil properties passed to quest constructor")
    end

    if(o.id == nil) then
        error("Property id not set")
    end
    self.__index = self

    o.data = o.data or {}
    o.tags = o.tags or {}
    
    setmetatable(o, self)
    return o
end

function Quest:ShowOptions(renderer, context, completed)
    renderer:AddText("quest."..self.id..".description")
    if (completed == false and self.main == false) then
        renderer:AddAction(function()
            RemoveQuest(self)
            context:ChangeState("ui_quest_book")
        end, "quest_book.quest.remove")
    end
end

function Quest:DisplayName()
    return GetTranslation("quest."..self.id..".name")
end

-- ============================
-- Quest Book UI
-- ============================
BlackListState("ui_quest_book")
BlackListState("ui_quest_details")

StateBuilder("ui_quest_book")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("DarkSlateGray")
                renderer:AddAction(function() context:ChangeState(OldState:Get()) end, "quest_book.close")
                -- show quests in progress
                renderer:AddText("quest_book.header.inProgress")
                for key, entry in ipairs(GetQuestBook()) do
                    if(entry.completed == false) 
                    then
                        renderer:AddAction(function()
                            activeDetailItem=entry
                            context:ChangeState("ui_quest_details")
                            end, "quest_book.quest", { { "quest", entry.quest:DisplayName() } } )
                    end
                end
                --show completed quests
                renderer:AddText("quest_book.header.completed")
                for key, entry in ipairs(GetQuestBook()) do
                    if(entry.completed == true) 
                    then
                        renderer:AddAction(function()
                            activeDetailItem=entry
                            context:ChangeState("ui_quest_details")
                            end, "quest_book.quest", { { "quest", entry.quest:DisplayName() } } )
                    end
                end
            end
        )
        :Build()

StateBuilder("ui_quest_details")
        :Sticky(false)
        :AllowSave(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor("DarkSlateGray")
                renderer:AddAction(function() context:ChangeState("ui_quest_book") end, "quest_book.quest.close")
                renderer:AddText("quest_book.quest.header", { { "quest", activeDetailItem.quest:DisplayName()} })
                activeDetailItem.quest:ShowOptions(renderer, context, activeDetailItem.completed)
            end
        )
        :Build()

Global:AddOnPostStateRender(
        function(renderer, context)
            if(IsUI(context.ActiveStateId)) then
                return
            end
            renderer:AddAction(function() context:ChangeState("ui_quest_book") end, "quest_book.open")
        end
)

-- ============================
-- EXPORTS
-- ============================
Context:CreateFunc("AddQuest", AddQuest)
Context:CreateFunc("CloseQuest", CloseQuest)
Context:CreateFunc("RemoveQuest", RemoveQuest)
Context:CreateFunc("IsQuestCompleted", IsQuestCompleted)
Context:CreateFunc("HasJustCompletedQuest", HasJustCompletedQuest)
Context:CreateFunc("GetQuestBook", GetQuestBook)
Context:CreateFunc("RegisterQuest", RegisterQuest)
Context:CreateFunc("GetQuest", GetQuest)
Context:CreateVar("Quest", Quest)