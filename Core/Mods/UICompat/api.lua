uiStates = {}
local oldState = Context:CreateVar("OldState", nil)

function BlackListState(stateId)
    table.insert(uiStates, stateId)
end

local function has_value (tab, val)
    for index, value in ipairs(tab) do
        if value == val then
            return true
        end
    end

    return false
end

-- ============================
-- Listeners
-- ============================
Global:AddOnPostStateRender(
        function(renderer, context)
            if(has_value(uiStates, context.ActiveStateId)) then
                return
            end

            oldState:Set(context.ActiveStateId);
        end
)

-- ============================
-- Exports
-- ============================

Context:CreateFunc("BlackListState", BlackListState)
Context:CreateFunc("IsUI", function (state) return has_value(uiStates, state) end)