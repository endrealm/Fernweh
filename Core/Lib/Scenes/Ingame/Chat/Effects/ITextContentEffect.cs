using Core.Utils;

namespace Core.Scenes.Ingame.Chat.Effects;

public interface ITextContentEffect: ITextEffect, IUpdate<ChatUpdateContext>
{
}