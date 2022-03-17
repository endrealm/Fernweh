StateBuilder("my_state")
        :Render(
                function(renderer, context) 
                    --[[local text = ]]
                    renderer:AddText("dialog.example.1")
                    renderer:AddAction(function() context:ChangeState("my_other_state") end, "dialog.example.1.button")
                    --renderer:addAction(StateChangeAction("example.button.1", "my_other_state"))
                    --renderer:addAction(CustomAction("example.button.1", 
                    --        function()
                    --            text:setMessage("example.message.3")
                    --            context:ClearActions()
                    --            context:runLater(1000*2, 
                    --                    function()  
                    --                        context:changeState("my_other_state")
                    --                    end
                    --            )
                    --        end
                    --))
                end
        )
        --.showExit(false) defaults to false anyways
        :Build()

StateBuilder("my_other_state")
        :Render(
                function(renderer, context)
                    renderer:SetBackgroundColor("DarkRed")
                    renderer:AddText("dialog.example.2")
                    renderer:AddAction(function() context:Exit() end, "dialog.example.2.button")
                end
        )
        :Build()