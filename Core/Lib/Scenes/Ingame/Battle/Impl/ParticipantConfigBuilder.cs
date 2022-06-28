using System.Collections.Generic;
using System.Globalization;

namespace Core.Scenes.Ingame.Battle.Impl;

public class ParticipantConfigBuilder
{
    private readonly string _id;
    private Stats _stats = new();
    private int _health = -1;
    private int _mana = -1;
    private List<AbilityConfig> _abilities = new();

    public ParticipantConfigBuilder(string id)
    {
        _id = id;
    }

    public ParticipantConfigBuilder CurrentHealth(int value)
    {
        _health = value;
        return this;
    }
    public ParticipantConfigBuilder CurrentMana(int value)
    {
        _mana = value;
        return this;
    }
    public ParticipantConfigBuilder Experience(int value)
    {
        _stats.Experience = value;
        return this;
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
    public ParticipantConfigBuilder Armor(int value)
    {
        _stats.Armor = value;
        return this;
    }

    public ParticipantConfigBuilder Dexterity(int value)
    {
        _stats.Dexterity = value;
        return this;
    }
    public ParticipantConfigBuilder Strength(int value)
    {
        _stats.Strength = value;
        return this;
    }
    public ParticipantConfigBuilder Constitution(int value)
    {
        _stats.Constitution = value;
        return this;
    }
    public ParticipantConfigBuilder Intellect(int value)
    {
        _stats.Intellect = value;
        return this;
    }
    public ParticipantConfigBuilder Wisdom(int value)
    {
        _stats.Wisdom = value;
        return this;
    }
    public ParticipantConfigBuilder Charisma(int value)
    {
        _stats.Charisma = value;
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
            _abilities,
            _health,
            _mana
        );
    }
}