local compat = Import("ui_compat", "api")
BlackListState = compat:GetFunc("BlackListState")

Global:AddOnPreStateRender(
        function(renderer, context)
            renderer:AddLabel(10, 10, "test.text")
        end
)

StateBuilder("my_state")
        :Render(
                function(renderer, context)
                    renderer:AddText("intro")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                    renderer:AddAction(function() context:StartBattle() end, "start.battle")
                end
        )
        :Build()

StateBuilder("enter_snow")
        :Render(
                function(renderer, context)
                    renderer:AddText("enter.snow")
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
                    renderer:AddText("text.castle.enter")
                    renderer:AddAction(function() context:ChangeState("castle_entrance") end, "button.castle.enter.entrance")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.1.button")
                end
        )
        :Build()

StateBuilder("castle_entrance")
        :Render(
                function(renderer, context)
                    renderer:AddText("text.castle.entrance")
                    renderer:AddAction(function() context:ChangeState("castle_corridor") end, "button.castle.enter.corridor")
                    renderer:AddAction(function() context:ChangeState("castle_shops") end, "button.castle.enter.shops")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.3.button")
                end
        )
        :Build()

StateBuilder("castle_corridor")
        :Render(
                function(renderer, context)
                    renderer:AddText("text.castle.guard.look")

                    renderer:AddAction(
                    function() 
                        context:ChangeState("castle_corridor_greet")
                    end, "button.castle.guards.greet")

                    renderer:AddAction(
                    function() 
                        context:ChangeState("castle_corridor_threaten")
                    end, "button.castle.guards.threaten")

                    renderer:AddAction(function() context:ChangeState("castle_entrance") end, "dialog.example.3.button")
                end
        )
        :Build()

StateBuilder("castle_corridor_greet")
        :Render(
                function(renderer, context)
                    renderer:AddText("text.castle.guard.greet") 
                    renderer:AddAction(function() context:ChangeState("castle_entrance") end, "dialog.example.3.button")
                end
        )
        :Build()

StateBuilder("castle_corridor_threaten")
        :Render(
                function(renderer, context)
                    renderer:AddText("text.castle.guard.threaten") 
                    renderer:AddAction(function() context:ChangeState("castle_entrance") end, "dialog.example.3.button")
                end
        )
        :Build()

StateBuilder("castle_shops")
        :Render(
                function(renderer, context)
                    renderer:AddText("text.castle.shops")
                    renderer:AddAction(function() context:ChangeState("castle_entrance") end, "dialog.example.3.button")
                end
        )
        :Build()

BlackListState("leave_snow")
StateBuilder("leave_snow")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.snow", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_forest")
StateBuilder("leave_forest")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.forest", function() context:Exit() end)
                end
        )
        :Build()


BlackListState("leave_path")
StateBuilder("leave_path")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.path", function() context:Exit() end)
                end
        )
        :Build()

BlackListState("leave_castle")
StateBuilder("leave_castle")
        :ClearScreenPost(false)
        :Render(
                function(renderer, context)
                    renderer:AddText("leave.castle", function() context:Exit() end)
                end
        )
        :Build()