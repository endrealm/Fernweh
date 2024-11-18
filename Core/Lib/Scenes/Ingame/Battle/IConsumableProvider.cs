using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IConsumableProvider
{
    List<IConsumable> Collect(BattleRegistry registry);
}