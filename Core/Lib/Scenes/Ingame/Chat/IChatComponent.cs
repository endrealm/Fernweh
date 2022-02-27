using Core.Utils;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.Chat;

public interface IChatComponent: IRenderer<ChatRenderContext>, IUpdate<ChatUpdateContext>
{
    Vector2 Dimensions { get; }
    float MaxWidth { set; }
}