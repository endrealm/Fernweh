using System.Collections.Generic;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class BattleActionsLuaBridge
{
    public IBattleAction CreateText(string key)
    {
        return new LogTextAction(key);
    }
    
    public IBattleAction CreateAwaitNext()
    {
        return new AwaitNextAction();
    }
    public IBattleAction CreateDoNothing(IBattleParticipant participant)
    {
        return new DoNothingAction(participant);
    }
    
    public IBattleAction CreateDamage(LuaTable data, IBattleParticipant actor, List<IBattleParticipant> targets)
    {
        return new DealDamageAction(new DamageData((int)(long)data["Damage"], (Element)(int)(long)data["Element"]), actor, targets);
    }
}