using Core.Utils;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.Chat;

public class ChatRenderContext: IRenderContext
{
    public Vector2 Position { get; }

    public ChatRenderContext(
        Vector2 position
    )
    {
        Position = position;
    }
}