using Core.Utils;
using System;
using System.Collections.Generic;

namespace Core.Scenes.Ingame.Chat.Effects;

public interface ITextContentEffect: ITextEffect, IUpdate<ChatUpdateContext>
{
    List<Action> OnFinish { get; set; }
}