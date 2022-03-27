using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame;

public class BattleMode: IMode
{
    public Color Background { get; } = new(18, 14, 18);

    public IChatView ChatView { get; }
    public IGameView GameView { get; }

    public BattleMode()
    {
        ChatView = new BaseChatView();
        GameView = new BattleGameView();
    }
}