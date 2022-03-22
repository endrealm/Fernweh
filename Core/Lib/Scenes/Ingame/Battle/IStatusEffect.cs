namespace Core.Scenes.Ingame.Battle;

public interface IStatusEffect : IBattleEventReceiver
{
    void OnTryCleanse(out bool persist);
}