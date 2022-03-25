using System.Collections.Generic;
using System.Globalization;

namespace Core.Scenes.Ingame.Battle.Impl;

public class ParticipantConfigBuilder
{
    private readonly string _id;
    private Stats _stats = new();
    private List<AbilityConfig> _abilities = new();

    public ParticipantConfigBuilder(string id)
    {
        _id = id;
    }

    public ParticipantConfigBuilder Health(int value)
    {
        _stats.Health = value;
        return this;
    }
    public ParticipantConfigBuilder Mana(int value)
    {
        _stats.Mana = value;
        return this;
    }
    
    public ParticipantConfigBuilder Agility(int value)
    {
        _stats.Agility = value;
        return this;
    }
    public ParticipantConfigBuilder Strength(int value)
    {
        _stats.Strength = value;
        return this;
    }
    public ParticipantConfigBuilder Defense(int value)
    {
        _stats.Defense = value;
        return this;
    }
    public ParticipantConfigBuilder Intellect(int value)
    {
        _stats.Intellect = value;
        return this;
    }
    public ParticipantConfigBuilder Spirit(int value)
    {
        _stats.Spirit = value;
        return this;
    }
    public ParticipantConfigBuilder Evasion(int value)
    {
        _stats.Evasion = value;
        return this;
    }
    
    
    public ParticipantConfigBuilder AddAbility(AbilityConfig config)
    {
        _abilities.Add(config);
        return this;
    }
        
    public ParticipantConfig Build()
    {
        return new ParticipantConfig(
            _id,
            _stats,
            _abilities
        );
    }
}