using System.Collections.Generic;
using Core.Scenes.Ingame.Battle.Impl;

namespace Core.Scenes.Ingame.Battle;

public class BattleRegistry
{
    private readonly Dictionary<string, IEffectFactory> _effectFactories = new();
    private readonly Dictionary<string, IParticipantFactory> _participantFactories = new();
    private readonly Dictionary<string, IAbilityFactory> _abilityFactories = new();
    public IFriendlyParticipantsProvider FriendlyParticipantsProvider {get; set;}
    private readonly List<IConsumableProvider> _consumableProviders = new();
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
    public void RegisterConsumableProvider(IConsumableProvider provider)
    {
        _consumableProviders.Add(provider);
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

    public List<IConsumable> CollectConsumables(BattleRegistry battleRegistry)
    {
        if (_consumableProviders.Count == 1) return _consumableProviders[0].Collect(battleRegistry);
        var results = new List<IConsumable>();

        _consumableProviders.ForEach(provider => results.AddRange(provider.Collect(battleRegistry)));

        return results;
    }
}