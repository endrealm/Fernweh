StateBuilder("my_state")
        :Render(
                function(renderer, context)
                    renderer:AddText("intro")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

StateBuilder("enter_grass")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.grass")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

StateBuilder("enter_forest")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.forest")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

StateBuilder("enter_path")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.path")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

StateBuilder("enter_castle")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.castle")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

StateBuilder("leave_grass")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.grass", function() context:Exit() end)
                end
        )
        :Build()

StateBuilder("leave_forest")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.forest", function() context:Exit() end)
                end
        )
        :Build()


StateBuilder("leave_path")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.path", function() context:Exit() end)
                end
        )
        :Build()

StateBuilder("leave_castle")
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.castle", function() context:Exit() end)
                end
        )
        :Build()
