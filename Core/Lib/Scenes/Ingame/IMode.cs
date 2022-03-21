using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame;

public interface IMode
{
    public Color Background { get; }
    public IChatView ChatView { get; }
    public GameView GameView { get; }
}