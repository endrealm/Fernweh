using System;
using System.Collections.Generic;
using Core.Utils;

namespace Core.Scenes.Ingame.Chat.Effects;

public interface ITextContentEffect : ITextEffect, IUpdate<ChatUpdateContext>
{
    List<Action> OnFinish { get; set; }
}