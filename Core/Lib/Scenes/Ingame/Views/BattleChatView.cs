using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Chat;
using Core.Utils;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Views;

public class BattleChatView: BaseChatView, IPlayerBattleInput
{
    private const int SelectionPhaseHeaderLength = 1;
    public BattleChatView(DialogTranslationData translationData, IFontManager fontManager) : base(translationData, fontManager)
    {
    }

    public BattleManager BattleManager { get; set; }
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
            if (!ability.CanUse(new AbilityUseCheckContext(participant)))
            {
                AddText("battle.ability." + ability.Id + ".blocked");
                return;
            }

            AddAction("battle.ability." + ability.Id, () =>
            {
                ShowTargeting(participant, ability, () => ShowAbilities(participant, categoryId, abilities));
            });
        });
        LoadNextComponentInQueue();
    }
    private void ShowTargeting(IBattleParticipant participant, IAbility ability, Action onBack)
    {
        Clear(SelectionPhaseHeaderLength);
        
        AddAction("battle.participant.list.back", onBack.Invoke);

        AddText("battle.select.target");

        GetTargetsByType(ability).ForEach(targets =>
        {
            var types = targets.Select(target => target.ParticipantId).ToSet();
            var countMode = (targets.Count > 0 ? "multiple" : "single");
            if (types.Count > 1) countMode = "mixed";
            
            AddAction("battle.participant.select." + countMode, () =>
            {
                Clear();
                participant.NextAction = ability.ProduceAction(participant, targets);
            }, new Replacement("amount", targets.Count.ToString()), new Replacement("name", string.Join(", ", types)));
        });
        LoadNextComponentInQueue();
    }

    private List<List<IBattleParticipant>> GetTargetsByType(IAbility ability)
    {
        switch (ability.TargetType)
        {
            case AbilityTargetType.FriendlySingle:
            {
                var type = new List<List<IBattleParticipant>>();
                BattleManager.Friendlies.ForEach(participant => type.Add(new() {participant}));
                return type;
            }
            case AbilityTargetType.EnemySingle:
            {
                var type = new List<List<IBattleParticipant>>();
                BattleManager.Enemies.ForEach(participant => type.Add(new() {participant}));
                return type;
            }
            case AbilityTargetType.EnemyGroup:
            {

                return BattleManager.Enemies
                    .GroupBy(participant => participant.ParticipantId)
                    .Select(grouping => grouping.ToList())
                    .ToList();
            }
            case AbilityTargetType.EnemyAll: return new()
            {
                BattleManager.Enemies
            };
            case AbilityTargetType.FriendlyGroup: return new() 
            {
                BattleManager.Friendlies
            };
            default:
            case AbilityTargetType.All: return new()
            {
                BattleManager.All
            };
        }
    }
}