local test = Context:CreateVariable("test")
test:Set("example")

Context:CreateFunction("test2", function()
    print("ez noob")
end)