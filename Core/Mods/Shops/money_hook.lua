Balance = Context:CreateStoredVar("Balance", 0)

Provider = Context:CreateVar("BalanceProvider", {
    GetMoney=function(self)
        return Balance:Get()
    end,
    SetMoney=function(self, newBal)
        Balance:Set(newBal)
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

function AddMoney(balance)
    SetMoney(GetMoney() + balance)
    return balance
end

Context:CreateFunc("GetMoney", GetMoney)
Context:CreateFunc("SetMoney", SetMoney)
Context:CreateFunc("HasMoney", HasMoney)
Context:CreateFunc("Purchase", Purchase)
Context:CreateFunc("AddMoney", AddMoney)