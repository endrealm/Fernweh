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

        var dict = new Dictionary<string, List<IAbility>>();
        participant.GetAbilities().ForEach(ability =>
        {
            if (dict.ContainsKey(ability.CategoryId))
            {
                dict[ability.CategoryId].Add(ability);
                return;
            }

            dict.Add(ability.CategoryId, new List<IAbility> { ability });
        });

        AddText("battle.participant.list.attack");
        AddText("battle.participant.list.defend");
        foreach (var pair in dict)
            AddAction("battle.participant.list.ability."+ pair.Key, () =>
            {
                ShowAbilities(participant, pair.Key, pair.Value);
            });
        AddText("battle.participant.list.items");

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

    private void ShowAbilities(IBattleParticipant participant, string categoryId, List<IAbility> abilities)
    {
        Clear(SelectionPhaseHeaderLength);
        
        AddAction("battle.participant.list.back", () =>
        {
            RenderRound(participant);
        });

        AddText("battle.ability.category." + categoryId);

        abilities.ForEach(ability =>
        {
            AddAction("battle.ability." + ability.Id, () =>
            {
                // todo: select participant action here
            });
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