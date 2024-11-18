---
order: 100
---

# Intro to states
## About
States are written in lua. A `state` is atomic and is in charge of creating interactible content in the "overworld".

This means a state is in charge of:
 - state rendering (mainly in chat)
 - offering conditional actions (and action logic)
 - blocking user movement

Most logic is handled in the `:Render` callback.

## Creating a new state
In your lua file you can create a new state with the following syntax:
```lua
StateBuilder("my_state")
    :Render(
        function(renderer, context) 
            -- add your render logic here
        end
    )
    :Build()
```
[!button variant="info" text="State Builder Docs"](/api/classes/state-builder.md)