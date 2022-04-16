-- ============================
-- Imports
-- ============================

local character = Import("lib/character")
local Character = character:Get("Character")

-- ============================
-- Core
-- ============================
Factories = {}

function RegisterCharacterType(type, factory)
    Factories[type] = factory
end

function LoadCharacterType(type, id, data)
    return Factories[type](id, data)
end

-- ============================
-- Defaults
-- ============================
RegisterCharacterType("default", function(id, data)
    local characterInstance = Character:new({id=id})

    if(data ~= nil) then
        characterInstance:Deserialize(data)
    end
    
    return characterInstance
end)
-- ============================
-- Exports
-- ============================
context:CreateFunc("RegisterCharacterType", RegisterCharacterType)
context:CreateFunc("LoadCharacterType", LoadCharacterType)

