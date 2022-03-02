﻿using Core.Utils;
using System;

namespace Core.Scenes.Ingame.Chat.Effects;

public interface ITextContentEffect: ITextEffect, IUpdate<ChatUpdateContext>
{
    Action _onFinish { get; set; }
}