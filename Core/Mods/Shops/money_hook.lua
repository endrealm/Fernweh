Balance = 0

Provider = Context:CreateVar("BalanceProvider", {
    GetMoney=function()
        return Balance
    end,
    SetMoney=function(newBal)
        Balance = newBal
    end
})


function GetMoney() 
    return Provider:Get():GetMoney()
end

function SetMoney(balance) 
    return Provider:Get():SetMoney(balance)
end

function HasMoney(balance) 
    return GetMoney() > balance
end

function Purchase(balance) 
    return SetMoney(GetMoney() - balance)
end
