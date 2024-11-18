---
order: 98
---

# Create Party

## Description
Creates and adds a character to the players party

## Example
```lua
-- Imports
local character = Import("character_system", "lib/character")
local party = Import("character_system", "lib/party")

local Character = character:Get("Character")
local AddToParty = party:GetFunc("AddToParty")

-- Add party member
AddToParty(Character:new({id = "Triss"}))
-- Giving starting stats example
AddToParty(Character:new({id = "Yennifer", stats = {health=12, mana=2, strength=8, intellect=10, dexterity=17, constitution=14, wisdom=9, charisma=12}}))
-- Giving starting equipment example
AddToParty(Character:new({id = "Luneiros", equip = {weapon="shortsword", body="leather_armor", feet="fur_boots"}}))
```