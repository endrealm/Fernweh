﻿using System.Threading.Tasks;
using Core.Scenes.Ingame.Chat;

namespace Core.Scenes.Ingame.Battle.Impl.Actions;

public class LogTextAction : IBattleAction
{
    private readonly string _key;
    private readonly Replacement[] _replacements;

    public LogTextAction(string key, params Replacement[] replacements)
    {
        _key = key;
        _replacements = replacements;
    }

    public IBattleParticipant Participant { get; }

    public async Task DoAction(ActionContext context)
    {
        var done = false;
        context.AddText(_key, () => done = true, _replacements);
        while (!done) await Task.Delay(50);
    }

    public int Priority => -1;
    public bool AllowDeath { get; } = true;
    public bool CausesStateCheck { get; } = false;

}