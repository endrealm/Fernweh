local maker = Import("States/variableMaker")
local test = maker:GetVar("test")
print(test:Get())

local test2 = maker:GetFunc("test2")
test2();

--local inventory = Import("inventory", "api")
--local Item = inventory:Get("Item")
--inventory:GetFunc("RegisterItem")(Item:new{id = "sample_item"})
--inventory:GetFunc("AddItem")("sample_item")