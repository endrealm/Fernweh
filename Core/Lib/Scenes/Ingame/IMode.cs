using Core.Scenes.Ingame.Modes;
using Core.Scenes.Ingame.Views;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame;

public interface IMode
{
    public Color Background { get; }
    public IChatView ChatView { get; }
    public IGameView GameView { get; }

    public void Load(ModeParameters parameters);
}