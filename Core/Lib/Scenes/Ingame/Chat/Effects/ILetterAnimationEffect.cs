using Core.Utils;
using Core.Utils.Math;

namespace Core.Scenes.Ingame.Chat.Effects;

/// <summary>
///     TODO make functional.
///     The idea is that this replaces the default line rendering algorithm and modifies the rendered values.
/// </summary>
public interface ILetterAnimationEffect : ITextEffect, IRenderer<ChatRenderContext>, IUpdate<ChatUpdateContext>
{
    float LastLineRemainingSpace { get; }
    float LastLineLength { get; }
    float LastLineHeight { get; }
    bool EmptyLineEnd { get; }

    /// <summary>
    ///     Shape starts at 0/0 of text not the world. Size is world coordinates, but you need to
    ///     consider the offset before performing any checks
    /// </summary>
    IShape Shape { get; }

    public void Recalculate();
    public float CalculateHeight();
    public float CalculateWidth();
}