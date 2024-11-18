using System;
using System.Collections.Generic;
using Core.Utils;
using Core.Utils.Math;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.Chat;

public interface IChatComponent : IRenderer<ChatRenderContext>, IUpdate<ChatUpdateContext>
{
    Vector2 Dimensions { get; }
    float MaxWidth { set; get; }
    IShape Shape { get; }
    void SetOnDone(Action action);
}

public interface IChatInlineComponent : IChatComponent
{
    float LastLineRemainingSpace { get; }
    float LastLength { get; }
    float LastLineHeight { get; }
    float FirstLineOffset { set; }
    bool DirtyContent { get; set; }
    bool EmptyLineEnd { get; }
}

public interface IChatContainerComponent : IChatComponent
{
    /// <summary>
    ///     Some containers might not use this!
    /// </summary>
    float MaxHeight { set; get; }

    float MaxContentWidth { set; get; }
    public void AppendComponents(List<IChatInlineComponent> chatInlineComponents);
    public void AppendComponent(IChatInlineComponent chatInlineComponents);
}