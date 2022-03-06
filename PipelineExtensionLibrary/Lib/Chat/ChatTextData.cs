using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PipelineExtensionLibrary.Chat;

public class ChatTextData: IChatComponentData
{
    public Color Color { get; }
    public string Text { get; }

    public ChatTextData(Color color, string text)
    {
        Color = color;
        Text = text;
    }

}