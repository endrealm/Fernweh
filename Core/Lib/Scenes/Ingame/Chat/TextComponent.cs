using System;
using Core.Scenes.Ingame.Chat.Effects;
using Core.Scenes.Ingame.Chat.Effects.Default;
using Core.Utils.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public class TextComponent : BaseComponent, IChatInlineComponent
{
    private readonly ITextContentEffect _contentEffect;
    private readonly ILetterAnimationEffect _letterAnimationEffect;
    private readonly float _width;
    private float _firstLineOffset;
    private float _maxWidth;

    public TextComponent(
        SpriteFont font,
        string message,
        float width = -1,
        float maxWidth = -1,
        ILetterAnimationEffect animationEffect = null,
        ITextContentEffect contentEffect = null
    ) : this(font, message, Color.White, width, maxWidth, animationEffect, contentEffect)
    {
    }

    public TextComponent(
        SpriteFont font,
        string message,
        Color textColor,
        float width = -1,
        float maxWidth = -1,
        ILetterAnimationEffect animationEffect = null,
        ITextContentEffect contentEffect = null
    )
    {
        Font = font;
        _width = width;
        _maxWidth = maxWidth;
        Message = message;
        TextColor = textColor;

        #region Content Effect

        if (contentEffect != null)
            _contentEffect = contentEffect;
        else
            _contentEffect = new StaticContentEffect();
        _contentEffect.Attach(this);

        #endregion

        #region Letter Animation Effect

        if (animationEffect != null)
            _letterAnimationEffect = animationEffect;
        else
            _letterAnimationEffect = new StaticLetterAnimationEffect();

        _letterAnimationEffect.Attach(this);
        _letterAnimationEffect.Recalculate();

        #endregion
    }

    public Color TextColor { get; }

    public SpriteFont Font { get; }

    public string Message { get; private set; }

    public override float Width => _width;

    public override void Render(SpriteBatch spriteBatch, ChatRenderContext context)
    {
        _letterAnimationEffect.Render(spriteBatch, context);
    }

    public override float MaxWidth
    {
        get => _maxWidth;
        set
        {
            _maxWidth = value;
            _letterAnimationEffect.Recalculate();
            DirtyContent = true;
        }
    }

    /// <summary>
    ///     Shape starts at 0/0 of text not the world. Size is world coordinates, but you need to
    ///     consider the offset before performing any checks
    /// </summary>
    public override IShape Shape => _letterAnimationEffect.Shape;

    public float LastLineRemainingSpace => _letterAnimationEffect.LastLineRemainingSpace;
    public float LastLength => _letterAnimationEffect.LastLineLength;
    public float LastLineHeight => _letterAnimationEffect.LastLineHeight;

    public float FirstLineOffset
    {
        get => _firstLineOffset;
        set
        {
            _firstLineOffset = value;
            Recalculate();
            DirtyContent = true;
        }
    }

    public bool DirtyContent { get; set; }
    public bool EmptyLineEnd => _letterAnimationEffect.EmptyLineEnd;

    public override void SetOnDone(Action action)
    {
        _contentEffect.OnFinish.Add(action);
    }

    public override void Update(float deltaTime, ChatUpdateContext context)
    {
        _contentEffect.Update(deltaTime, context);
        _letterAnimationEffect.Update(deltaTime, context);
    }

    public void ChangeMessage(string message)
    {
        Message = message;
        Recalculate();
        DirtyContent = true;
    }

    public void Recalculate()
    {
        _letterAnimationEffect?.Recalculate();
    }

    protected override float CalculateHeight()
    {
        return _letterAnimationEffect.CalculateHeight();
    }

    protected override float CalculateWidth()
    {
        return _letterAnimationEffect.CalculateWidth();
    }
}