using System.Collections.Generic;
using Core.Scenes.Ingame.Battle.Impl;

namespace Core.Scenes.Ingame.Battle;

public class BattleRegistry
{
    private readonly Dictionary<string, IEffectFactory> _effectFactories = new();
    private readonly Dictionary<string, IParticipantFactory> _participantFactories = new();
    private readonly Dictionary<string, IAbilityFactory> _abilityFactories = new();
    public IFriendlyParticipantsProvider FriendlyParticipantsProvider {get; set;}
    public void RegisterEffect(IEffectFactory effectFactory)
    {
        _effectFactories.Add(effectFactory.EffectId, effectFactory);
    }

    public void RegisterParticipant(IParticipantFactory factory)
    {
        _participantFactories.Add(factory.Id, factory);
    }

    public void RegisterAbility(IAbilityFactory factory)
    {
        _abilityFactories.Add(factory.Id, factory);
    }

    public IEffectFactory GetEffectFactory(string id)
    {
        return _effectFactories[id];
    }

    public IParticipantFactory GetParticipantFactory(string id)
    {
        return _participantFactories[id];
    }

    public IAbilityFactory GetAbilityFactory(string id)
    {
        return _abilityFactories[id];
    }
}