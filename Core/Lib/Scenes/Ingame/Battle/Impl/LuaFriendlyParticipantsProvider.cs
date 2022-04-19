using System.Collections.Generic;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaFriendlyParticipantsProvider: IFriendlyParticipantsProvider
{
    private List<LuaFunction> _luaFunctions = new();
    
    
    public List<ParticipantConfig> Load()
    {
        ParticipantConfigBuilder CreateBuilder(string id)
        {
            return new ParticipantConfigBuilder(id);
        }
        LuaAbilityConfigBuilder CreateAbilityBuilder(string id)
        {
            return new LuaAbilityConfigBuilder(id);
        }
        
        var participants = new List<ParticipantConfig>();

        foreach (var luaFunction in _luaFunctions)
        {
            var values = luaFunction.Call(CreateBuilder, CreateAbilityBuilder);
            foreach (var value in values)
            {
                ReadValue(participants, value);
            }
        }

        return participants;
    }

    public void RegisterFriendlyParticipantsProvider(LuaFunction luaFunction)
    {
        _luaFunctions.Add(luaFunction);
    }

    private static void ReadValue(ICollection<ParticipantConfig> participants, object value)
    {
        if (value is ParticipantConfig participantConfig)
        {
            participants.Add(participantConfig);
            return;
        }

        if (value is not LuaTable table)
        {
            return;
        }
        foreach (var tableValue in table.Values)
        {
            ReadValue(participants, tableValue);
        }
    }
}