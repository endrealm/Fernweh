using System.Threading.Tasks;

namespace Core.Scenes.Ingame.Battle;

public interface IPlayerBattleInput
{
    Task HandlePlayerInput();
}