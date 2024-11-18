using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class StaticLetterAnimationEffect : ILetterAnimationEffect
{
    private float _calculatedHeight;
    private float _calculatedWidth;
    private TextComponent _component;
    private List<string> _lines = new();

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
        var list = new List<IShape>();
        var i = -1;
        foreach (var line in _lines)
        {
            i++;
            if (line.Length == 0)
            {
                var heightVal = _component.Font.MeasureString("A").Y;
                sum += heightVal;
                LastLineLength = 0;
                LastLineHeight = heightVal;
                continue;
            }

            var (width, height) = _component.Font.MeasureString(line);
            if (highestWidth < width) highestWidth = width;
            list.Add(new RectangleShape(new Vector2(i == 0 ? _component.FirstLineOffset : 0, sum), width, height));
            sum += height;
            LastLineRemainingSpace = width - widthBound;
            LastLineLength = width;
            LastLineHeight = height;
        }

        if (_lines.Count == 1)
            // this means we might be in midst a string so we need to add the previous part length
            LastLineLength += _component.FirstLineOffset;

        _calculatedWidth = highestWidth;
        _calculatedHeight = sum;
        Shape = new CompoundShape(list);
    }

    public float CalculateHeight()
    {
        return _calculatedHeight;
    }

    public float CalculateWidth()
    {
        return _calculatedWidth;
    }

    public float LastLineRemainingSpace { get; private set; }

    public float LastLineLength { get; private set; }
    public float LastLineHeight { get; private set; }
    public bool EmptyLineEnd => _lines.Count == 0 || _lines.Last().Length == 0;
    public IShape Shape { get; private set; } = new CompoundShape(new List<IShape>());

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

            spriteBatch.DrawString(_component.Font, line,
                context.Position + new Vector2(i == 0 ? _component.FirstLineOffset : 0, heightOffset),
                _component.TextColor);
            heightOffset += _component.Font.MeasureString(line).Y;
        }
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
    }

    /// <summary>
    ///     Handles line breaking! Should be reworked to prevent mid word breaks if not configured to do so!
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected List<string> BreakLines(string message)
    {
        if (_component.Width < 0 && _component.MaxWidth < 0) return new List<string> {message};

        var remaining = message;
        var accumulator = "";
        var results = new List<string>();

        var width = GetWidthBound();

        while (remaining.Length > 0)
        {
            var newChar = remaining[0];
            if (_component.Font.MeasureString(accumulator + newChar).X >
                width - (results.Count == 0 ? _component.FirstLineOffset : 0))
            {
                if (accumulator.Length != 0 && remaining.Length != 1 && remaining[0] != ' ' &&
                    accumulator[accumulator.Length - 1] != ' ')
                {
                    var parts = accumulator.Split(' ');
                    if (parts.Length != 1)
                    {
                        accumulator = "";
                        for (var i = 0; i < parts.Length - 1; i++) accumulator += parts[i] + " ";

                        accumulator = accumulator.Substring(0, accumulator.Length - 1);
                        remaining = parts[parts.Length - 1] + remaining;
                        newChar = remaining[0];
                    }
                }

                results.Add(accumulator);
                accumulator = "";
            }

            if (newChar != ' ' || results.Count == 0 || accumulator.Length != 0) accumulator += newChar;

            remaining = remaining.Substring(1);
        }

        if (accumulator.Length > 0) results.Add(accumulator);

        return results;
    }

    private float GetWidthBound()
    {
        return _component.Width < 0 || _component.MaxWidth < 0
            ? Math.Max(_component.Width, _component.MaxWidth)
            : Math.Min(_component.Width, _component.MaxWidth);
    }
}