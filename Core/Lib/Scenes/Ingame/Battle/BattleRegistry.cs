using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class BattleRegistry
{
    private readonly Dictionary<string, IEffectFactory> _factories = new();
    
    public void Register(IEffectFactory effectFactory)
    {
        _factories.Add(effectFactory.EffectId, effectFactory);
    }

    public IEffectFactory GetFactory(string id)
    {
        return _factories[id];
    }
}