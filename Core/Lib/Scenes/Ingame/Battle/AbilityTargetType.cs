using System.Collections.Generic;
using System.Linq;

namespace Core.Scenes.Ingame.Battle;

public enum AbilityTargetType
{
    FriendlySingle,
    FriendlyGroup,
    EnemySingle,
    EnemyGroup,
    EnemyAll,
    All
}

public static class TargetTypeUtils
{
    public static List<List<IBattleParticipant>> GetTargetsByType(
        List<IBattleParticipant> enemies,
        List<IBattleParticipant> friendlies, 
        List<IBattleParticipant> all, 
        AbilityTargetType targetType
    )    {
        switch (targetType)
        {
            case AbilityTargetType.FriendlySingle:
            {
                var type = new List<List<IBattleParticipant>>();
                friendlies.ForEach(participant => type.Add(new() {participant}));
                return type;
            }
            case AbilityTargetType.EnemySingle:
            {
                var type = new List<List<IBattleParticipant>>();
                enemies.ForEach(participant => type.Add(new() {participant}));
                return type;
            }
            case AbilityTargetType.EnemyGroup:
            {

                return enemies
                    .GroupBy(participant => participant.GroupId)
                    .Select(grouping => grouping.ToList())
                    .ToList();
            }
            case AbilityTargetType.EnemyAll: return new()
            {
                enemies.ToList()
            };
            case AbilityTargetType.FriendlyGroup: return new() 
            {
                friendlies.ToList()
            };
            default:
            case AbilityTargetType.All: return new()
            {
                all.ToList()
            };
        }
    }
}