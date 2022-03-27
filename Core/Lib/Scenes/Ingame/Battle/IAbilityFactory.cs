namespace Core.Scenes.Ingame.Battle;

public interface IAbilityFactory
{
    public string Id { get; }
    public IAbility Produce(AbilityConfig config);
}