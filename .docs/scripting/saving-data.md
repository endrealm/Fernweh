---
order: 91
---

# Saving/Loading Data

## The save callback
Besides the automatic variable saving mechanism introduced in [ScriptReference](./script-reference.md#save-variables) its also possible to save data that is not stored in a variable. You may register <u>one save callback per script</u>! This can be achieved with the following code:
```lua
SetDataSaver(function()
    -- run your logic here
    return {
        some = "data",
        someOther = {"data1", "data2"}
    }
end)
```
[!button variant="info" text="Data Save Function Docs"](/api/functions/data-save-function.md)

## The load callback
The loadcallback is called in order of script execution **AFTER** all scripts have been executed! This allows to ensure that everything has been registered! You may register <u>one load callback per script</u>! Do not that the data value will be `nil` when no data has been saved previously!

```lua
SetDataLoader(function(data)
    if(data == nil) then
        -- set default data here
        return
    end
    -- run your logic here. Data is whatever you saved
end)
```

[!button variant="info" text="Data Load Function Docs"](/api/functions/data-load-function.md)
