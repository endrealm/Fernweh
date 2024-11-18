using Core.Utils;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.Chat;

public class ChatRenderContext : IRenderContext
{
    public ChatRenderContext(
        Vector2 position
    )
    {
        Position = position;
    }

    public Vector2 Position { get; }
}