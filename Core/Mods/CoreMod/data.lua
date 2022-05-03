local equip = Import("character_system", "lib/equip")
RegisterSlot = equip:GetFunc("RegisterSlot")

RegisterSlot("weapon")
RegisterSlot("helmet")
RegisterSlot("body")
RegisterSlot("cape")
RegisterSlot("boots")
RegisterSlot("accessory")
RegisterSlot("accessory")
RegisterSlot("accessory")
RegisterSlot("accessory")
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
