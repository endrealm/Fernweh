local equip = Import("character_system", "lib/equip")
RegisterSlot = equip:GetFunc("RegisterSlot")

RegisterSlot("weapon")
RegisterSlot("head")
RegisterSlot("body")
RegisterSlot("feet")
RegisterSlot("accessory")