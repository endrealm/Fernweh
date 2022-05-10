local equip = Import("character_system", "lib/equip")
RegisterSlot = equip:GetFunc("RegisterSlot")

RegisterSlot("weapon")
RegisterSlot("head")
RegisterSlot("body")
RegisterSlot("feet")
RegisterSlot("accessory")


-- test code below pls remove
SetDataLoader(function(data)
    print("Loaded data:")
    print(data)
end)

SetDataSaver(function()
    -- run your logic here
    return "test value inserted"
end)
-- test code above pls remove
