
-- ============================
-- Imports
-- ============================

local inventory = Import("inventory", "api")
GetItem = inventory:GetFunc("GetItem")
AddItemToInventory = inventory:GetFunc("AddItem")
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
BackgroundColor = "Brown"

ItemToBuy = {
    itemId= "example",
    price=10,
}

Settings = {
    exitState = "example",
    currencyName = "gold",
    offer = {
        {
            itemId= "example",
            price=10,
        }
    },
}

StateBuilder("ui_shop")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor(BackgroundColor)
                renderer:AddAction(function() context:ChangeState(Settings.exitState) end, "shop.close")
                renderer:AddText("shop.welcome")
                renderer:AddAction(function() context:ChangeState("ui_shop_buy") end, "shop.buy")
                -- renderer:AddAction(function() context:ChangeState("ui_shop_sell") end, "shop.sell")
            end
        )
        :Build()

StateBuilder("ui_shop_buy")
        :Sticky(false)
        :Render(
            function(renderer, context)
                renderer:SetBackgroundColor(BackgroundColor)
                renderer:AddAction(function() context:ChangeState("ui_shop") end, "shop.back")
                renderer:AddText("shop.buy.title", {"balance", GetMoney()})

                for _, offeredItem in ipairs(Settings.offer) do
                    renderer:AddAction(function()
                        ItemToBuy = offeredItem
                        context:ChangeState("shop_shop_buy_amount")
                    end, "shop.buy.item", {
                        {"item", GetItem(offeredItem.itemId):DisplayName()}, 
                        {"price", offeredItem.price}, 
                        {"currency", Settings.currencyName}
                    })
    
                end
                
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
                renderer:AddText("shop.buy.amount.title", {"balance", GetMoney()})
                
                function CreatePurchaseOption(amount)
                    if(HasMoney(ItemToBuy.price * amount)) then
                        renderer:AddAction(function()
                            -- purchase item
                            Purchase(ItemToBuy.price * amount)
                            AddItemToInventory(ItemToBuy.itemId, amount)
                            context:ChangeState("ui_shop_buy")
                        end, "shop.buy.amount", {
                            {"amount", amount},
                            {"price", ItemToBuy.price * amount},
                            {"currency", Settings.currencyName}
                        })
                    else
                        renderer:AddText("shop.buy.amount.insufficient_funds", {
                            {"amount", amount},
                            {"price", ItemToBuy.price * amount},
                            {"currency", Settings.currencyName}
                        })
                    end
                end
            end
        )
        :Build()

function OpenShop(context, settings)
    Settings = settings
    context:ChangeState("ui_shop")
end


-- ============================
-- Export
-- ============================
Context:CreateFunc("OpenShop", OpenShop)