using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripting;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaConsumableProvider: IConsumableProvider
{

    private readonly LuaFunction _collectorFunction;

    public LuaConsumableProvider(LuaFunction collectorFunction)
    {
        _collectorFunction = collectorFunction;
    }

    public List<IConsumable> Collect(BattleRegistry registry)
    {
        var consumablesRaw = (LuaTable) _collectorFunction.Call().First();
        var list = new List<IConsumable>();

        foreach (KeyValuePair<object, object> entry in consumablesRaw)
        {
            if (entry.Value is not LuaTable raw)
            {
                Console.Error.WriteLine("Tried to pass non object as consumable: " + entry.Value);
                continue;
            }

            try
            {
                var abilityId = (string) raw["ability"];
                var data = raw["abilityData"];
                list.Add(new LuaConsumable(
                    (int)(long)raw["amount"],
                    ((WrappedTranslation) raw["name"]).Content,
                    registry.GetAbilityFactory(abilityId).Produce(new AbilityConfig(abilityId, data)),
                    (LuaFunction) raw["onUse"]
                ));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                continue;
            }
        }
        
        return list;
    }
}