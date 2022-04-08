local maker = Import("States/variableMaker")
local test = maker:Get("test")
print(test:Get())

local test2 = maker:GetFunc("test2")
test2();