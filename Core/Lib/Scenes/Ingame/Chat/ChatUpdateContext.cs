using Core.Utils;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.Chat;

public class ChatUpdateContext: IUpdateContext
{
    public ChatUpdateContext(IngameUpdateContext ingameUpdateContext, Vector2 position)
    {
        IngameUpdateContext = ingameUpdateContext;
        Position = position;
    }

    public IngameUpdateContext IngameUpdateContext { get; }
    public Vector2 Position { get; }
}