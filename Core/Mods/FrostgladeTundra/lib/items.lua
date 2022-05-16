﻿-- ============================
-- Imports
-- ============================

local equip = Import("character_system", "lib/equip")
local EquipItem = equip:Get("EquipItem")

local inventory = Import("inventory", "api")
local RegisterItem = inventory:GetFunc("RegisterItem")
local AddItem = inventory:GetFunc("AddItem")

-- ============================
-- Items
-- ============================

-- Head

RegisterItem(EquipItem:new({
    id = "toque",
    slot="head",
    abilities = {
        stats = {
            armor=1,
            charisma=1,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "fur_hood",
    slot="head",
    abilities = {
        stats = {
            armor=2,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "wizards_hat",
    slot="head",
    abilities = {
        stats = {
            armor=2,
            wisdom=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "leather_cap",
    slot="head",
    abilities = {
        stats = {
            armor=3,
        },
    },
}))


RegisterItem(EquipItem:new({
    id = "irom_helm",
    slot="head",
    abilities = {
        stats = {
            armor=5,
        },
    },
}))

-- Body

RegisterItem(EquipItem:new({
    id = "clothes",
    slot="body",
    abilities = {
        stats = {
            armor=1,
            charisma=1,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "wizards_robe",
    slot="body",
    abilities = {
        stats = {
            armor=2,
            wisdom=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "sages_robe",
    slot="body",
    abilities = {
        stats = {
            armor=3,
            wisdom=5,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "leather_armor",
    slot="body",
    abilities = {
        stats = {
            armor=4,
            charisma=1,
        },
    },
}))


RegisterItem(EquipItem:new({
    id = "scaled_armor",
    slot="body",
    abilities = {
        stats = {
            armor=6,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "iron_armor",
    slot="body",
    abilities = {
        stats = {
            armor=8,
        },
    },
}))

-- Feet

RegisterItem(EquipItem:new({
    id = "shoes",
    slot="feet",
    abilities = {
        stats = {
            dexterity=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "fur_boots",
    slot="feet",
    abilities = {
        stats = {
            armor=1,
            dexterity=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "leather_boots",
    slot="feet",
    abilities = {
        stats = {
            armor=2,
            dexterity=2,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "plated_boots",
    slot="feet",
    abilities = {
        stats = {
            armor=3,
            dexterity=1,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "iron_boots",
    slot="feet",
    abilities = {
        stats = {
            armor=4,
        },
    },
}))

-- Accessories

RegisterItem(EquipItem:new({
    id = "ruby_ring",
    slot="accessory",
    price= 10,
    sellPrice= 5,
    abilities = {
        stats = {
            charisma=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "silver_armlet",
    slot="accessory",
    price= 10,
    sellPrice= 5,
    abilities = {
        stats = {
            strength=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "gold_necklace",
    slot="accessory",
    price= 10,
    sellPrice= 5,
    abilities = {
        stats = {
            constitution=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "pearl_earrings",
    slot="accessory",
    price= 10,
    sellPrice= 5,
    abilities = {
        stats = {
            dexterity=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "book_charm",
    slot="accessory",
    price= 10,
    sellPrice= 5,
    abilities = {
        stats = {
            intellect=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "holy_talisman",
    slot="accessory",
    price= 10,
    sellPrice= 5,
    abilities = {
        stats = {
            wisdom=3,
        },
    },
}))

-- Weapons

RegisterItem(EquipItem:new({
    id = "spear",
    slot="weapon",
    price= 20,
    sellPrice= 15,
    abilities = {
        stats = {
            strength=4,
            dexterity=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "halberd",
    slot="weapon",
    price= 30,
    sellPrice= 25,
    abilities = {
        stats = {
            strength=7,
            dexterity=2,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "shortsword",
    slot="weapon",
    price= 20,
    sellPrice= 15,
    abilities = {
        stats = {
            strength=6,
            dexterity=1,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "longsword",
    slot="weapon",
    price= 30,
    sellPrice= 25,
    abilities = {
        stats = {
            strength=8,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "knife",
    slot="weapon",
    price= 10,
    sellPrice= 5,
    abilities = {
        stats = {
            strength=2,
            dexterity=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "dagger",
    slot="weapon",
    price= 15,
    sellPrice= 10,
    abilities = {
        stats = {
            strength=4,
            dexterity=2,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "wand",
    slot="weapon",
    price= 15,
    sellPrice= 10,
    abilities = {
        stats = {
            intellect=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "wooden_staff",
    slot="weapon",
    price= 15,
    sellPrice= 15,
    abilities = {
        stats = {
            intellect=6,
            wisdom=1,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "sages_staff",
    slot="weapon",
    price= 30,
    sellPrice= 25,
    abilities = {
        stats = {
            intellect=8,
            wisdom=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "infinity_blade",
    slot="weapon",
    price= 35,
    sellPrice= 30,
    abilities = {
        stats = {
            strength=8,
            dexterity=3,
        },
    },
}))

RegisterItem(EquipItem:new({
    id = "lethality_blade",
    slot="weapon",
    price= 35,
    sellPrice= 30,
    abilities = {
        stats = {
            strength=10,
        },
    },
}))