---
order: 92
---

# Global Events
The global event handler acts as an interface to subscribe to various events, like PreStateRender and PostStateRender. It is supplied as the global variable `Global`. Generally allowing to subscribe via a callback function. Below will be an example that shows how to subscribe to a callback. The other events can be seen by looking at the GlobalEventHandler documentation.

[!button variant="info" text="Global Event Handler Docs"](/api/classes/global-event-handler.md)

```lua
Global:AddOnPostStateRender(
        function(renderer, context)
            -- add your render logic here like you would for a regular state
        end
)
```