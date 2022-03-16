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
    local display = renderer:CreateOrGetScreenLabel("dayDisplay")
    display:SetText("common.day_time", Replacement("day", day), Replacement("time", hour + ":00"))
    renderer:SetBackgroundColor(getColorFromTime())
end

stateBuilder("my_state")
    :render(
        function(renderer, context)
            
            -- Do normal render
            renderer:AddText("dialog.example")
            renderer:AddText("dialog.example")
            renderer:AddAction(function()
                context:ChangeState("other.state")
            end, "dialog.example.button")
        end
    )
    :Build()

-- this is called before every state render
global.AddOnPreStateRender(function(renderer, context)
    updateDayTime(renderer)
end)

-- this is called before every state render
global.AddOnPrePlayerMove(function()
    cycleTime()
end)