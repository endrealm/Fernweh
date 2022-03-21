stateBuilder("my_state")
        :Render(
                function(renderer, context)
                    renderer:AddText("intro")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

stateBuilder("enter_grass")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.grass")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

stateBuilder("enter_forest")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.forest")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

stateBuilder("enter_path")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.path")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

stateBuilder("enter_castle")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.castle")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

stateBuilder("leave_grass")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.grass", function() context:Exit() end)
                end
        )
        :Build()

stateBuilder("leave_forest")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.forest", function() context:Exit() end)
                end
        )
        :Build()


stateBuilder("leave_path")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.path", function() context:Exit() end)
                end
        )
        :Build()

stateBuilder("leave_castle")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.castle", function() context:Exit() end)
                end
        )
        :Build()
