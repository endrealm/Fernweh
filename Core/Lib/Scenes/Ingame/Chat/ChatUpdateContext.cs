using Core.Utils;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.Chat;

public class ChatUpdateContext: IUpdateContext
{
    public ChatUpdateContext(IngameUpdateContext ingameUpdateContext, Vector2 position, bool clickHandled)
    {
        IngameUpdateContext = ingameUpdateContext;
        Position = position;
        ClickHandled = clickHandled;
    }

    public IngameUpdateContext IngameUpdateContext { get; }
    public Vector2 Position { get; }
    public bool ClickHandled { get; set; }
}