using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Views;
using Core.Utils;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleChatView: BaseChatView, IPlayerBattleInput
{
    private const int SelectionPhaseHeaderLength = 1;
    public BattleChatView(ILocalizationManager localizationManager, IFontManager fontManager) : base(localizationManager, fontManager)
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
            if(ability.Hidden) return;
            if (dict.ContainsKey(ability.CategoryId))
            {
                dict[ability.CategoryId].Add(ability);
                return;
            }
            dict.Add(ability.CategoryId, new List<IAbility> { ability });
        });

        AddAction("battle.participant.list.attack", () =>
        {
            ShowTargeting(true, false, false, AbilityTargetType.EnemySingle, () => RenderRound(participant),
                (targets) =>
                {
                    Clear();
                    participant.NextAction = new AttackAction(participant, targets.First());
                });
        });
        AddAction("battle.participant.list.defend", () =>
        {
            Clear();
            participant.NextAction = new DefendAction(participant);
        });
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
            new TextReplacement("name", participant.DisplayName),
            new TextReplacement("health", participant.Health.ToString()),
            new TextReplacement("max_health", stats.Health.ToString()),
            new TextReplacement("mana", participant.Mana.ToString()),
            new TextReplacement("max_mana", stats.Mana.ToString())
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
            if(ability.Hidden) return;
            
            if (!ability.CanUse(new AbilityUseCheckContext(participant)))
            {
                if(ability.HideBlocked) return;
                AddText("battle.ability." + ability.Id + ".blocked");
                return;
            }

            AddAction("battle.ability", () =>
            {
                ShowTargeting(ability.AllowLivingTargets, ability.AllowDeadTargets, IsGroupType(ability), ability.TargetType, () => ShowAbilities(participant, categoryId, abilities),
                    (targets) =>
                    {
                        Clear();
                        participant.NextAction = ability.ProduceAction(participant, targets);
                    });
            }, new TextReplacement("ability", ability.Id));
        });
        LoadNextComponentInQueue();
    }
    private void ShowTargeting(bool allowLivingTargets, bool allowDeadTargets, bool showGroup, AbilityTargetType targetType, Action onBack, Action<List<IBattleParticipant>> onSelect)
    {
        Clear(SelectionPhaseHeaderLength);
        
        AddAction("battle.participant.list.back", onBack.Invoke);

        AddText("battle.select.target");
        GetTargetsByType(targetType).ForEach(targets =>
        {
            // Filter for dead targets if the spell does not allow this
            if (!allowDeadTargets)
            {
                targets.RemoveAll(target => target.State == ParticipantState.Dead);
            }
            // Filter for living if only dead are allowed
            if (!allowLivingTargets)
            {
                targets.RemoveAll(target => target.State == ParticipantState.Alive);
            }
            
            // skip now empty target groupings
            if(targets.Count == 0) return;
            
            var types = targets.Select(target => showGroup ? target.GroupId : target.DisplayName).ToSet();
            var countMode = (targets.Count > 1 ? "multiple" : "single");
            if (types.Count > 1) countMode = "mixed";
            
            AddAction("battle.participant.select." + countMode, () =>
            {
                onSelect.Invoke(targets);
            }, new TextReplacement("amount", targets.Count.ToString()), new TextReplacement("name", string.Join(", ", types)));
        });
        LoadNextComponentInQueue();
    }

    private bool IsGroupType(IAbility ability)
    {
        var type = ability.TargetType;
        return type == AbilityTargetType.EnemyGroup;
    }

    private List<List<IBattleParticipant>> GetTargetsByType(AbilityTargetType targetType)
    {
        switch (targetType)
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
                    .GroupBy(participant => participant.GroupId)
                    .Select(grouping => grouping.ToList())
                    .ToList();
            }
            case AbilityTargetType.EnemyAll: return new()
            {
                BattleManager.Enemies.ToList()
            };
            case AbilityTargetType.FriendlyGroup: return new() 
            {
                BattleManager.Friendlies.ToList()
            };
            default:
            case AbilityTargetType.All: return new()
            {
                BattleManager.All.ToList()
            };
        }
    }
}