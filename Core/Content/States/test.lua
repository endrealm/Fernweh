stateBuilder("my_state")
        :render(
                function(renderer, context) 
                    --[[local text = ]]
                    renderer:addText("dialog.example.1")
                    renderer:addAction(function() context:changeState("my_other_state") end, "dialog.example.1.button")
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
        :build()

stateBuilder("my_other_state")
        :render(
                function(renderer, context)
                    renderer:addText("dialog.example.2")
                    renderer:addAction(function() context:exit() end, "dialog.example.2.button")
                end
        )
        :build()