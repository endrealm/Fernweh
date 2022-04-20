-- ============================
-- Imports
-- ============================

local character = Import("character_system", "lib/character")
local party = Import("character_system", "lib/party")

local Character = character:Get("Character")
local AddToParty = party:GetFunc("AddToParty")

-- ============================
-- Temp testing
-- ============================
AddToParty(Character:new({id = "Yennifer", stats = {health=10, mana=5}}))
AddToParty(Character:new({id = "Triss"}))
AddToParty(Character:new({id = "Geralt"}))
AddToParty(Character:new({id = "Ciri"}))