using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Battle.Impl.Actions;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Views;
using Core.Utils;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Modes.Battle;

public class BattleChatView : BaseChatView, IPlayerBattleInput
{
    private const int SelectionPhaseHeaderLength = 1;
    private List<IConsumable> _consumables;

    public BattleChatView(ILocalizationManager localizationManager, IFontManager fontManager) : base(
        localizationManager, fontManager)
    {
    }

    public BattleManager BattleManager { get; set; }

    public async Task HandlePlayerInput(List<IBattleParticipant> battleParticipants)
    {
        _consumables = BattleManager.Registry.CollectConsumables(BattleManager.Registry);
        foreach (var battleParticipant in battleParticipants) await HandlePlayerInput(battleParticipant);

        _consumables = null; // Clear references
    }

    private async Task HandlePlayerInput(IBattleParticipant participant)
    {
        await Task.Delay(30);
        Clear();
        RenderRound(participant);

        while (participant.NextAction == null) await Task.Delay(50);
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
            if (ability.Hidden) return;
            if (dict.ContainsKey(ability.CategoryId))
            {
                dict[ability.CategoryId].Add(ability);
                return;
            }

            dict.Add(ability.CategoryId, new List<IAbility> {ability});
        });

        AddAction("battle.participant.list.attack", () =>
        {
            ShowTargeting(true, false, false, AbilityTargetType.EnemySingle, () => RenderRound(participant),
                targets =>
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
            AddAction("battle.participant.list.ability." + pair.Key,
                () => { ShowAbilities(participant, pair.Key, pair.Value); });
        AddAction("battle.participant.list.items", () => { ShowItems(participant); });

        LoadNextComponentInQueue();
    }

    private void PrintSelectionHeader(IBattleParticipant participant)
    {
        var stats = participant.GetStats();
        AddText("battle.participant.list.name",
            new TextReplacement("name", participant.DisplayName),
            new TextReplacement("health", participant.Health.ToString()),
            new TextReplacement("max_health", stats.Health.ToString()),
            new TextReplacement("mana", participant.Mana.ToString()),
            new TextReplacement("max_mana", stats.Mana.ToString())
        );
    }

    private void ShowItems(IBattleParticipant participant)
    {
        Clear(SelectionPhaseHeaderLength);

        AddAction("battle.participant.list.back", () => { RenderRound(participant); });

        if (_consumables.Count == 0) AddText("battle.participant.no_items");

        _consumables.ForEach(consumable =>
        {
            AddAction("battle.participant.list.item", () =>
                {
                    var ability = consumable.Ability;
                    ShowTargeting(ability.AllowLivingTargets, ability.AllowDeadTargets, IsGroupType(ability),
                        ability.TargetType, () => ShowItems(participant),
                        targets =>
                        {
                            Clear();
                            participant.NextAction = consumable.ProduceAction(participant, targets);
                        });
                }, new WrapperReplacement("item", consumable.Name),
                new TextReplacement("amount", consumable.Amount.ToString()));
        });
        LoadNextComponentInQueue();
    }

    private void ShowAbilities(IBattleParticipant participant, string categoryId, List<IAbility> abilities)
    {
        Clear(SelectionPhaseHeaderLength);

        AddAction("battle.participant.list.back", () => { RenderRound(participant); });

        AddText("battle.ability.category." + categoryId);

        abilities.ForEach(ability =>
        {
            if (ability.Hidden) return;

            if (!ability.CanUse(new AbilityUseCheckContext(participant)))
            {
                if (ability.HideBlocked) return;
                AddText("battle.ability." + ability.Id + ".blocked");
                return;
            }

            AddAction("battle.ability", () =>
            {
                ShowTargeting(ability.AllowLivingTargets, ability.AllowDeadTargets, IsGroupType(ability),
                    ability.TargetType, () => ShowAbilities(participant, categoryId, abilities),
                    targets =>
                    {
                        Clear();
                        participant.NextAction = ability.ProduceAction(participant, targets);
                    });
            }, new TextReplacement("ability", ability.Id));
        });
        LoadNextComponentInQueue();
    }

    private void ShowTargeting(bool allowLivingTargets, bool allowDeadTargets, bool showGroup,
        AbilityTargetType targetType, Action onBack, Action<List<IBattleParticipant>> onSelect)
    {
        Clear(SelectionPhaseHeaderLength);

        AddAction("battle.participant.list.back", onBack.Invoke);

        AddText("battle.select.target");
        GetTargetsByType(targetType).ForEach(targets =>
        {
            // Filter for dead targets if the spell does not allow this
            if (!allowDeadTargets) targets.RemoveAll(target => target.State == ParticipantState.Dead);
            // Filter for living if only dead are allowed
            if (!allowLivingTargets) targets.RemoveAll(target => target.State == ParticipantState.Alive);

            // skip now empty target groupings
            if (targets.Count == 0) return;

            var types = targets.Select(target => showGroup ? target.GroupId : target.DisplayName).ToSet();
            var countMode = targets.Count > 1 ? "multiple" : "single";
            if (types.Count > 1) countMode = "mixed";

            AddAction("battle.participant.select." + countMode, () => { onSelect.Invoke(targets); },
                new TextReplacement("amount", targets.Count.ToString()),
                new TextReplacement("name", string.Join(", ", types)));
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
        return TargetTypeUtils.GetTargetsByType(BattleManager.Enemies, BattleManager.Friendlies, BattleManager.All,
            targetType);
    }
}