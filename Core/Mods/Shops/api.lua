
-- ============================
-- Imports
-- ============================

local inventory = Import("inventory", "api")
GetItem = inventory:GetFunc("GetItem")
GetInventoryStack = inventory:GetFunc("GetInventoryStack")
AddItemToInventory = inventory:GetFunc("AddItem")
RemoveItemFromInventory = inventory:GetFunc("RemoveItem")
GetInventory = inventory:GetFunc("GetInventory")

local uiCompat = Import("ui_compat", "api")
BlackListState = uiCompat:GetFunc("BlackListState")

local money = Import("money_hook")
GetMoney = money:GetFunc("GetMoney")
HasMoney = money:GetFunc("HasMoney")
Purchase = money:GetFunc("Purchase")

-- ============================
-- UI
-- ============================
BlackListState("ui_shop")
BlackListState("ui_shop_buy")
BlackListState("shop_shop_buy_amount")
BlackListState("ui_shop_sell")
BlackListState("shop_shop_sell_amount")

BackgroundColor = "Brown"

ItemToBuy = Context:CreateStoredVar("ItemToBuy", {
    itemId= "example",
    price= 10,
})

ItemToSell = Context:CreateStoredVar("ItemToSell", "example")

Settings = Context:CreateStoredVar("Settings", {
    exitState = "example",
    currencyName = "gold",
    offer = {
        {
            itemId= "example",
            price=10,
        }
    },
})

StateBuilder("ui_shop")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor(BackgroundColor)
                renderer:AddAction(function() context:ChangeState(Settings:Get().exitState) end, "shop.close")
                renderer:AddText("shop.welcome")
                renderer:AddAction(function() context:ChangeState("ui_shop_buy") end, "shop.buy")
                renderer:AddAction(function() context:ChangeState("ui_shop_sell") end, "shop.sell")
            end
        )
        :Build()

StateBuilder("ui_shop_buy")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor(BackgroundColor)
                renderer:AddAction(function() context:ChangeState("ui_shop") end, "shop.back")
                renderer:AddText("shop.buy.title", { { "balance", GetMoney() } })
                renderer:AddText("")
                for _, offeredItem in ipairs(Settings:Get().offer) do
                    local basePrice = offeredItem.price or GetItem(offeredItem.itemId).price or 0;

                    renderer:AddAction(function()
                        ItemToBuy:Set(offeredItem)
                        context:ChangeState("shop_shop_buy_amount")
                    end, "shop.buy.item", {
                        {"item", GetItem(offeredItem.itemId):DisplayName()}, 
                        {"price", basePrice}, 
                        {"currency", Settings:Get().currencyName}
                    })

                end
                renderer:AddText("")
                renderer:AddAction(function() context:ChangeState("ui_shop") end, "shop.back")
            end
        )
        :Build()

StateBuilder("shop_shop_buy_amount")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor(BackgroundColor)
                renderer:AddAction(function() context:ChangeState("ui_shop_buy") end, "shop.back")
                renderer:AddText("shop.buy.amount.title", { { "balance", GetMoney() } })
                
                local basePrice = ItemToBuy:Get().price or GetItem(ItemToBuy:Get().itemId).price or 0;

                local function CreatePurchaseOption(amount)
                    
                    local multiPrice = basePrice * amount;
                    if(HasMoney(multiPrice)) then
                        renderer:AddAction(function()
                            -- purchase item
                            Purchase(multiPrice)
                            AddItemToInventory(ItemToBuy:Get().itemId, amount)
                            context:ChangeState("ui_shop_buy")
                        end, "shop.buy.amount", {
                            {"amount", amount},
                            {"price", multiPrice},
                            {"currency", Settings:Get().currencyName}
                        })
                    else
                        renderer:AddText("shop.buy.amount.insufficient_funds", {
                            {"amount", amount},
                            {"price", basePrice * amount},
                            {"currency", Settings:Get().currencyName}
                        })
                    end
                end
                
                CreatePurchaseOption(1)
                CreatePurchaseOption(5)
                CreatePurchaseOption(10)
            end
        )
        :Build()



StateBuilder("ui_shop_sell")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor(BackgroundColor)
                renderer:AddAction(function() context:ChangeState("ui_shop") end, "shop.back")
                renderer:AddText("shop.sell.title", { { "balance", GetMoney() } })
                renderer:AddText("")
                for _, stack in ipairs(GetInventory()) do
                    local basePrice = Settings:Get().purchase[stack.item.id] or stack.item.sellPrice;
                    if(basePrice ~= nil) then
                        renderer:AddAction(function()
                            ItemToSell:Set(stack.item.id)
                            context:ChangeState("shop_shop_sell_amount")
                        end, "shop.sell.item", {
                            {"item", stack.item:DisplayName()},
                            {"price", basePrice},
                            {"currency", Settings:Get().currencyName}
                        }) 
                    end
                end
                renderer:AddText("")
                renderer:AddAction(function() context:ChangeState("ui_shop") end, "shop.back")
            end
        )
        :Build()

StateBuilder("shop_shop_sell_amount")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor(BackgroundColor)
                renderer:AddAction(function() context:ChangeState("ui_shop_sell") end, "shop.back")
                local item = GetItem(ItemToSell:Get())
                local basePrice = Settings:Get().purchase[item.id] or item.sellPrice;
                renderer:AddText("shop.sell.amount.title", { { "item", item:DisplayName() } })
                renderer:AddText("shop.sell.amount.current", { { "amount", GetInventoryStack(item.id) } })
    
                local function CreatePurchaseOption(amount)
    
                    local multiPrice = basePrice * amount;
                    if(GetInventoryStack(item) >= amount) then
                        renderer:AddAction(function()
                            -- purchase item
                            Purchase(-multiPrice)
                            RemoveItemFromInventory(ItemToBuy:Get().itemId, amount)
                            context:ChangeState("ui_shop_sell")
                        end, "shop.sell.amount", {
                            {"amount", amount},
                            {"price", multiPrice},
                            {"currency", Settings:Get().currencyName}
                        })
                    end
                end
    
                CreatePurchaseOption(1)
                CreatePurchaseOption(5)
                CreatePurchaseOption(10)
            end
        )
        :Build()
function OpenShop(context, settings)
    Settings:Set(merge({
        exitState = "null",
        currencyName = "Gold",
        offer = {},
        purchase = {}
    }, settings))
    context:ChangeState("ui_shop")
end



-- ============================
-- External Utilities
-- ============================
-- from: https://stackoverflow.com/a/7470789/9866079
function merge(t1, t2)
    for k, v in pairs(t2) do
        if (type(v) == "table") and (type(t1[k] or false) == "table") then
            merge(t1[k], t2[k])
        else
            t1[k] = v
        end
    end
    return t1
end


-- ============================
-- Export
-- ============================
Context:CreateFunc("OpenShop", OpenShop)