using Core.Utils;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.Chat;

public interface IChatComponent: IRenderer<ChatRenderContext>, IUpdate<ChatUpdateContext>
{
    Vector2 Dimensions { get; }
    float MaxWidth { set; }
}

public interface IChatInlineComponent: IChatComponent
{
    float LastLineRemainingSpace { get; }
    float LastLength { get; }
    float LastLineHeight { get; }
    float FirstLineOffset { set; }
    bool DirtyContent { get; set; }
    bool EmptyLineEnd { get; }
}