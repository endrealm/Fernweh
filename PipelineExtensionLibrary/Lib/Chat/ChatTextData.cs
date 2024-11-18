using Microsoft.Xna.Framework;

namespace PipelineExtensionLibrary.Chat;

public class ChatTextData : IChatComponentData
{
    public ChatTextData(Color color, string text)
    {
        Color = color;
        Text = text;
    }

    public Color Color { get; }
    public string Text { get; }
}