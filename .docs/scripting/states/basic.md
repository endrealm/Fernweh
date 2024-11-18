---
order: 95
---
# Basic Dialogue Example
Now with a .lang file, we can use new functions like `renderer:AddText()` and `renderer:AddAction()`. Because we setup the [grass tile](/map-creation/maps/) in the map to call an "enter_grass" lua state, we can create that state to be called for when you enter the grass tile.

```lua
StateBuilder("enter_grass")
        :Render(
                function(renderer, context) 
                    renderer:AddText("grave.1")
                    renderer:AddAction(function() context:ChangeState("demon_surpise") end, "grave.button.1")
                end
        )
        :Build()

StateBuilder("demon_surpise")
        :Render(
                function(renderer, context)
                    renderer:SetBackgroundColor("DarkRed")
                    renderer:AddText("grave.2")
                    renderer:AddAction(function() context:Exit() end, "grave.button.2")
                end
        )
        :Build()
```

!!!
Remember to add your lua scripts to the [index.json](/setup/new-proj/) to be loaded!
!!!

The .lang file would look like this:
```json
{
  "grave.1": {
    "translations": {
      "EN_US": "You stand at the entrance to a <color=\"Gray\">nightmarish</color> grave."
    }
  },
  "grave.2": {
    "translations": {
      "EN_US": "A <color=\"Red\">HUGE</color> demon rises up in front of you. His horns are larger than your whole body. He turns towards you a <color=\"Purple\">vicious smile</color> on his face..."
    }
  },
  "grave.button.1": {
    "translations": {
      "EN_US": "Walk <color=\"Green\">forward</color> slowly!"
    }
  },
  "grave.button.2": {
    "translations": {
      "EN_US": "<color=\"Red\">Run for your life!</color>"
    }
  }
}
```