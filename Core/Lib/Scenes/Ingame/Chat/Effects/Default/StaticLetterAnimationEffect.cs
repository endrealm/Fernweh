using System;
using System.Collections.Generic;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class StaticLetterAnimationEffect: ILetterAnimationEffect
{
    private TextComponent _component;
    private List<string> _lines = new();
    private float _calculatedWidth;
    private float _calculatedHeight = 0;

    
    public void Attach(TextComponent component)
    {
        _component = component;
    }
    
    public void Recalculate()
    {
        _lines = BreakLines(_component.Message);
        float sum = 0;
        float highestWidth = 0;
        foreach (var line in _lines)
        {
            var (width, height) = _component.Font.MeasureString(line);
            if (highestWidth < width) highestWidth = width;
            sum += height;
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
        var width = Math.Max(_component.Width, _component.MaxWidth);
        
        while (remaining.Length > 0)
        {
            var newChar = remaining[0];
            if (_component.Font.MeasureString(accumulator + newChar).X > width)
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

    public void Render(SpriteBatch spriteBatch, ChatRenderContext context)
    {
        var heightOffset = 0f;
        for (var i = 0; i < _lines.Count; i++)
        {
            var line = _lines[i];
            spriteBatch.DrawString(_component.Font, line, context.Position + new Vector2(0, heightOffset), _component.TextColor);
            heightOffset += _component.Font.MeasureString(line).Y;
        }
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        
    }
}