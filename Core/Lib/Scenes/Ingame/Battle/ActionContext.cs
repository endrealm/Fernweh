using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;

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

    public ActionContext AddText(string key, Action onDone, Replacement[] replacements)
    {
        Console.WriteLine("Show text: " + key);
        return this;
    }

    public ActionContext AddAction(string key, Action onClick, Replacement[] replacements)
    {
        Console.WriteLine("Show action: " + key);
        return this;
    }
}