﻿using System;
using Core.Utils;
using Core.Utils.Math;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Core.Scenes.Ingame.Chat;

public interface IChatComponent: IRenderer<ChatRenderContext>, IUpdate<ChatUpdateContext>
{
    Vector2 Dimensions { get; }
    float MaxWidth { set; }
    IShape Shape { get; }
    void SetOnDone(Action action);
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