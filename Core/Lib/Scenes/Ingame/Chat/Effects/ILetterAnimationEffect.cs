using Core.Utils;

namespace Core.Scenes.Ingame.Chat.Effects;

/// <summary>
/// TODO make functional.
/// The idea is that this replaces the default line rendering algorithm and modifies the rendered values.
/// </summary>
public interface ILetterAnimationEffect: ITextEffect, IRenderer<ChatRenderContext>, IUpdate<ChatUpdateContext>
{
    public void Recalculate();
    public float CalculateHeight();
    public float CalculateWidth();
    float LastLineRemainingSpace { get; }
    float LastLineLength { get; }
    float LastLineHeight { get; }
    bool EmptyLineEnd { get; }
}