using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame;

public interface IMode
{
    public Color Background { get; }
    public ChatView ChatView { get; }
    public GameView GameView { get; }
}