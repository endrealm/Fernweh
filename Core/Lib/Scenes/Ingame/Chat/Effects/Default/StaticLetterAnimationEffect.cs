using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class StaticLetterAnimationEffect: ILetterAnimationEffect
{
    private TextComponent _component;
    private List<string> _lines = new();
    private float _calculatedWidth;
    private float _calculatedHeight = 0;
    private float _calculatedLastLineRemainingSpace = 0;

    
    public void Attach(TextComponent component)
    {
        _component = component;
    }
    
    public void Recalculate()
    {
        _lines = BreakLines(_component.Message);
        float sum = 0;
        float highestWidth = 0;
        var widthBound = GetWidthBound();
        foreach (var line in _lines)
        {
            if (line.Length == 0)
            {
                sum += _component.Font.MeasureString("A").Y;
                continue;
            }
            var (width, height) = _component.Font.MeasureString(line);
            if (highestWidth < width) highestWidth = width;
            sum += height;
            _calculatedLastLineRemainingSpace = width - widthBound;
            LastLineLength = width;
            LastLineHeight = height;
        }

        if (_lines.Count == 1)
        {
            // this means we might be in midst a string so we need to add the previous part length
            LastLineLength += _component.FirstLineOffset;
        }

        _calculatedWidth = highestWidth;
        _calculatedHeight = sum;
    }

    public float CalculateHeight()
    {
        return _calculatedHeight;
    }

    public float CalculateWidth()
    {
        return _calculatedWidth;
    }

    public float LastLineRemainingSpace => _calculatedLastLineRemainingSpace;
    public float LastLineLength { get; private set; }
    public float LastLineHeight { get; private set; }

    /// <summary>
    /// Handles line breaking! Should be reworked to prevent mid word breaks if not configured to do so!
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected List<string> BreakLines(string message)
    {
        if (_component.Width < 0 && _component.MaxWidth < 0)
        {
            return new List<string> { message };
        }
        
        var remaining = message;
        var accumulator = "";
        var results = new List<string>();

        float width = GetWidthBound();

        while (remaining.Length > 0)
        {
            var newChar = remaining[0];
            if (_component.Font.MeasureString(accumulator + newChar).X > (width - (results.Count == 0 ? _component.FirstLineOffset : 0)))
            {
                results.Add(accumulator);
                accumulator = "";
            }

            accumulator += newChar;
            remaining = remaining.Substring(1);
        }

        if(accumulator.Length > 0) results.Add(accumulator);
        
        return results;
    }

    private float GetWidthBound()
    {
        return _component.Width < 0 || _component.MaxWidth < 0
            ? Math.Max(_component.Width, _component.MaxWidth)
            : Math.Min(_component.Width, _component.MaxWidth);
    }

    public void Render(SpriteBatch spriteBatch, ChatRenderContext context)
    {
        var heightOffset = 0f;
        for (var i = 0; i < _lines.Count; i++)
        {
            var line = _lines[i];
            if (line.Length == 0)
            {
                heightOffset += _component.Font.MeasureString("A").Y;
                continue;
            }
            spriteBatch.DrawString(_component.Font, line, context.Position + new Vector2(i == 0 ? _component.FirstLineOffset : 0, heightOffset),
                _component.TextColor);
            heightOffset += _component.Font.MeasureString(line).Y;
        }
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        
    }
}