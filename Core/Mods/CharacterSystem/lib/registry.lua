Factories = {}

function RegisterCharacterType(id, factory)
    Factories[id] = factory
end

function LoadCharacterType(id)
    return Factories[id](id)
end

-- ============================
-- EXPORTS
-- ============================
context:CreateFunc("RegisterCharacterType", RegisterCharacterType)
context:CreateFunc("LoadCharacterType", LoadCharacterType)