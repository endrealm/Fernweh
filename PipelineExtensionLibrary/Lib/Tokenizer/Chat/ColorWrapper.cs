using Microsoft.Xna.Framework;

namespace PipelineExtensionLibrary.Tokenizer.Chat;

public class ColorWrapper: ChatWrapper
{
    public ColorWrapper(Color color)
    {
        Color = color;
    }

    public Color Color { get; }

    public override MergeResult Merge(MergeResult parent)
    {
        var merge = parent.Clone();
        merge.Color = Color;
        return merge;
    }
    
    public override ChatWrapper Clone()
    {
        var wrapper = new ColorWrapper(Color);
        CloneInto(wrapper);
        return wrapper;
    }
}