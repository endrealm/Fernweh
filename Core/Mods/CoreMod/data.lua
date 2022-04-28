local equip = Import("character_system", "lib/equip")
RegisterSlot = equip:GetFunc("RegisterSlot")

RegisterSlot("weapon")
RegisterSlot("helmet")
RegisterSlot("body")
RegisterSlot("boots")