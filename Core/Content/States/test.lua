stateBuilder("my_state")
        :render(
                function(renderer, context) 
                    --[[local text = ]]
                    renderer:addText("dialog.example")
                    renderer:addText("dialog.example")
                    renderer:addAction(function() print("example") end, "dialog.example.button")
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
                    renderer:addText("example.message.4")
                    renderer:addAction(StateChangeAction("example.button.1", "my_state"))
                    renderer:addAction(CustomAction("example.button.3",
                            function()
                                context:exit();
                            end
                    ))
                end
        )
        :build()