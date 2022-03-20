using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class BattleRegistry
{
    private readonly Dictionary<string, IEffectFactory> _effectFactories = new();
    private readonly Dictionary<string, IParticipantFactory> _participantFactories = new();

    public void RegisterEffect(IEffectFactory effectFactory)
    {
        _effectFactories.Add(effectFactory.EffectId, effectFactory);
    }

    public void RegisterParticipant(IParticipantFactory factory)
    {
        _participantFactories.Add(factory.Id, factory);
    }

    public IEffectFactory GetEffectFactory(string id)
    {
        return _effectFactories[id];
    }

    public IParticipantFactory GetParticipantFactory(string id)
    {
        return _participantFactories[id];
    }
}