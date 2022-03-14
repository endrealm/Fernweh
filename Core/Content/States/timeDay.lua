day = 0
hour = 0

function cycleTime()
    hour = hour + 1;
    if(hour >=24) then
        hour = 0;
        day = day + 1;
    end
end

function getColorFromTime() 
    -- proper code ofc
    return "rgba(12,112,12,0.5)"
end

function updateDayTime(renderer)
    local display = renderer:createOrGetScreenLabel("dayDisplay")
    display.setText("common.day_time", Replacement("day", day), Replacement("time", hour + ":00"))
    renderer:setBackgroundColor(getColorFromTime())
end

stateBuilder("my_state")
    :render(
        function(renderer, context)
            
            -- Do normal render
            renderer:addText("dialog.example")
            renderer:addText("dialog.example")
            renderer:addAction(function()
                context:changeState("other.state")
            end, "dialog.example.button")
        end
    )
    :build()

-- this is called before every state render
global.addOnPreStateRender(function(renderer, context)
    updateDayTime(renderer)
end)

-- this is called before every state render
global.addOnPrePlayerMove(function()
    cycleTime()
end)