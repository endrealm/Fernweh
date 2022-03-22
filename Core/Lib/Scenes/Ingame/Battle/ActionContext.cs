using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public class ActionContext
{
    private readonly List<IBattleAction> _actions = new();

    public IEnumerable<IBattleAction> GetActionList()
    {
        return _actions;
    }

    public void QueueAction(IBattleAction battleAction)
    {
        _actions.Add(battleAction);
    }
}