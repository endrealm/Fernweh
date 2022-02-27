using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Chat.Effects;
using Core.Scenes.Ingame.Chat.Effects.Default;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public class TextComponent: BaseComponent
{
    private readonly SpriteFont _font;
    private float _width;
    private float _maxWidth;
    private string _message;
    private readonly Color _textColor;
    private ILetterAnimationEffect _letterAnimationEffect;
    private ITextContentEffect _contentEffect;

    public Color TextColor => _textColor;
    public SpriteFont Font => _font;
    public string Message => _message;

    public TextComponent(
        SpriteFont font,
        string message,
        float width = -1,
        float maxWidth = -1,
        ILetterAnimationEffect animationEffect = null,
        ITextContentEffect contentEffect = null
    ): this(font, message, Color.White, width, maxWidth, animationEffect, contentEffect) { }
    
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
        _font = font;
        _width = width;
        _maxWidth = maxWidth;
        _message = message;
        _textColor = textColor;

        #region Content Effect

        if (contentEffect != null)
        {
            _contentEffect = contentEffect;
        }
        else
        {
            _contentEffect = new StaticContentEffect();
        }
        _contentEffect.Attach(this);

        #endregion

        #region Letter Animation Effect

        if (animationEffect != null)
        {
            _letterAnimationEffect = animationEffect;
        }
        else
        {
            _letterAnimationEffect = new StaticLetterAnimationEffect();
        }
        
        _letterAnimationEffect.Attach(this);
        _letterAnimationEffect.Recalculate();

        #endregion
    }
    
    public void ChangeMessage(string message)
    {
        _message = message;
        Recalculate();
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
        }
    }

    public override float Width => _width;
    public override void Update(float deltaTime, ChatUpdateContext context)
    {
        _contentEffect.Update(deltaTime, context);
        _letterAnimationEffect.Update(deltaTime, context);
    }
}