using System;
using Microsoft.Xna.Framework;

namespace PipelineExtensionLibrary;

public static class ColorParser
{
    public static Color ToColor(this string rawColor)
    {
        var prop = typeof(Color).GetProperty(rawColor);
        Color color;
        if (prop != null && rawColor != "A" && rawColor != "R" && rawColor != "G" && rawColor != "B")
        {
            color = (Color) prop.GetValue(null, null);
        }
        else if (rawColor.StartsWith("rgb(") && rawColor.EndsWith(")"))
        {
            rawColor = rawColor.Substring(4, rawColor.Length - 5);
            var parts = rawColor.Split(',');
            if (parts.Length != 3) throw new Exception("Invalid RGB color: " + rawColor);
            color = new Color(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
        else if (rawColor.StartsWith("rgba(") && rawColor.EndsWith(")"))
        {
            rawColor = rawColor.Substring(5, rawColor.Length - 6);
            var parts = rawColor.Split(',');
            if (parts.Length != 4) throw new Exception("Invalid RGBA color: " + rawColor);
            color = new Color(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])) *
                    float.Parse(parts[3].Replace(".", ","));
        }
        else
        {
            throw new Exception("Invalid color!");
        }

        return color;
    }
}