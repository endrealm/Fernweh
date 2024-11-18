using Microsoft.Xna.Framework;

namespace PipelineExtensionLibrary.Tokenizer.Chat;

public class MergeResult
{
    public string Text { get; set; }
    public Color Color { get; set; }

    public MergeResult Clone()
    {
        return new MergeResult
        {
            Text = Text,
            Color = Color
        };
    }
}