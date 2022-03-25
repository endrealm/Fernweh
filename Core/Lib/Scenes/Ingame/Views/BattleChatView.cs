using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Views;

public class BattleChatView: BaseChatView, IPlayerBattleInput
{
    public BattleChatView(DialogTranslationData translationData, IFontManager fontManager) : base(translationData, fontManager)
    {
    }
    public async Task HandlePlayerInput(List<IBattleParticipant> battleParticipants)
    {
        foreach (var battleParticipant in battleParticipants)
        {
            await HandlePlayerInput(battleParticipant);
        }
    }

    private async Task HandlePlayerInput(IBattleParticipant participant)
    {
        await Task.Delay(30);
        RenderRound(participant);

        while (participant.NextAction == null)
        {
            await Task.Delay(50);
        }
    }

    private void RenderRound(IBattleParticipant participant)
    {
        Clear();
        AddText($"participant.{participant.ParticipantId}.name");
        AddAction("sample.action", () =>
        {
            Clear();
            participant.NextAction = new TestAction(participant);
        });
        LoadNextComponentInQueue();
    }
}

public class TestAction : IBattleAction
{
    public TestAction(IBattleParticipant participant)
    {
        Participant = participant;
    }

    public IBattleParticipant Participant { get; }
    public async Task DoAction(ActionContext context)
    {
        Console.WriteLine(Participant.ParticipantId + " do test action");
    }

    public int Priority => 1;
}