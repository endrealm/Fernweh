local maker = Import("States/variableMaker")
local test = maker:GetVar("test")
print(test:Get())

local test2 = maker:GetFunc("test2")
test2();

local inventory = Import("Scripts/inventory")
local Item = inventory:Get("Item")
