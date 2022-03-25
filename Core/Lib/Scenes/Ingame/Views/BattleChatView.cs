using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Chat;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Views;

public class BattleChatView: BaseChatView, IPlayerBattleInput
{
    private const int SelectionPhaseHeaderLength = 1;
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
        Clear();
        RenderRound(participant);

        while (participant.NextAction == null)
        {
            await Task.Delay(50);
        }
    }

    private void RenderRound(IBattleParticipant participant)
    {
        Clear(SelectionPhaseHeaderLength);
        if (RunningComponents.Count != SelectionPhaseHeaderLength)
        {
            Clear();
            PrintSelectionHeader(participant);
        }
        
        AddAction("battle.participant.list.abilities", () =>
        {
            ShowAbilities(participant);
        });
        
        AddAction("battle.participant.list.skills", () =>
        {
            ShowSkills(participant);
        });
        LoadNextComponentInQueue();
    }

    private void PrintSelectionHeader(IBattleParticipant participant)
    {
        var stats = participant.GetStats();
        AddText($"battle.participant.list.name", 
            new Replacement("name", participant.ParticipantId),
            new Replacement("health", participant.Health.ToString()),
            new Replacement("max_health", stats.Health.ToString()),
            new Replacement("mana", participant.Mana.ToString()),
            new Replacement("max_mana", stats.Mana.ToString())
        );
    }

    private void ShowAbilities(IBattleParticipant participant)
    {
        Clear(SelectionPhaseHeaderLength);
        
        AddAction("battle.participant.list.back", () =>
        {
            RenderRound(participant);
        });
        
        AddText("abilties should go here");
        LoadNextComponentInQueue();
    }
    private void ShowSkills(IBattleParticipant participant)
    {
        Clear(SelectionPhaseHeaderLength);
        
        AddAction("battle.participant.list.back", () =>
        {
            RenderRound(participant);
        });
        
        AddText("skills should go here");
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