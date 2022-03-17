using Core.Utils;

namespace Core.Scenes.Ingame.Battle;

public interface IEffectFactory
{
    public string EffectId { get; }
    public IStatusEffect Produce(IBattleParticipant target, PropsArray props);
}