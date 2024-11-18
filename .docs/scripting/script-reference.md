---
order: 93
---

# Referencing Scripts/Code Splitting
When creating content you will quickly notice that your files start getting quite large and hard to read. To aid you in writing clean and maintainable code we allow splitting code and import/export pieces.

## Importing
Importing allows to reference variables and functions exported by other classes or classes you dependent on.
Do note that cyclic dependencies are not allowed. While its possible to import a script that hasn't been loaded or is loaded after the current script, this will lead to the import object to be empty until the other script is issued.

**Import a file from same mod:**
```lua
local module = Import("path/to/my/module") -- path should NOT end with .lua
```
**Import a file from another mod:**
```lua
local module = Import("modId", "path/to/my/module")
```
The return value alias `module` in the example is a Store Reader instance.

[!button variant="info" text="Store Reader Docs"](/api/classes/store-reader.md)

### Read raw types (e.g. classes)
To read a raw type for example a class use the `:Get(name)` method. While it is currently possible to also fetch functions this behaviour might be removed later on.

```lua
local Item = module:Get("Item")

```

### Read safe variables
Variables are wrapped into an object to allow controlling their usage and behaviour.

```lua
local someVariable = module:Get("someVariable")
local value = someVariable:Get() -- get current value
someVariable:Set(value) -- set value
```
### Read functions

```lua
local SomeFunction = module:GetFunc("SomeFunction")
SomeFunction(value)
```

## Exporting
Exporting variables and functions allows other scripts and mods to access and use them.

```lua
Context:CreateVar("VariableName", Variable)
Context:CreateFunc("FuncName", Function)
```

[!button variant="info" text="Store Writer Docs"](/api/classes/store-writer.md)

### Save variables
To easily save variables into the gamesave you simply use `CreateStoredVar`
```lua
Context:CreateStoredVar("VariableName", Variable)
```
This will automatically save the variable content. Do be aware that the variable **now only supports Tables and primitives**!