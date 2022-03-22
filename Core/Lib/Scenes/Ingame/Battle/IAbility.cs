namespace Core.Scenes.Ingame.Battle;

public interface IAbility : IBattleEventReceiver
{
    void Use(AbilityUseContext context);
    bool CanUse(AbilityUseContext context);
}