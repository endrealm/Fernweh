using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Modes;
using Core.Scenes.Ingame.Views;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Battle;

public class ActionContext
{
    private readonly IChatView _chatView;
    private readonly List<IBattleAction> _actions = new();

    public ActionContext(IChatView chatView)
    {
        _chatView = chatView;
    }
    
    public IEnumerable<IBattleAction> GetActionList()
    {
        return _actions;
    }

    public void QueueAction(IBattleAction battleAction)
    {
        _actions.Add(battleAction);
    }

    public ActionContext AddText(string key, Action onDone, params IReplacement[] replacements)
    {
        var item = _chatView.AddText(key, replacements);
        item.SetOnDone(onDone);
        _chatView.ForceLoadNext();
        return this;
    }

    public ActionContext AddAction(string key, Action onClick, params IReplacement[] replacements)
    {
        _chatView.AddAction(key, onClick, replacements);
        _chatView.ForceLoadNext();
        return this;
    }

    public void ClearChat()
    {
        _chatView.Clear();
    }
}