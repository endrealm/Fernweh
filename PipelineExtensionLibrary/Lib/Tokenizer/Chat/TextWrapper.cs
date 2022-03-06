using System.Collections.Generic;
using System.Linq;

namespace PipelineExtensionLibrary.Tokenizer.Chat;

public class TextWrapper: ChatWrapper
{
    public TextWrapper(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public override List<MergeResult> Flatten(MergeResult parent)
    {
        
        
        var children =  base.Flatten(parent);

        children.Insert(0, Merge(parent));
        
        return children;
    }

    public override MergeResult Merge(MergeResult parent)
    {
        var merge = parent.Clone();
        merge.Text = Text;
        return merge;
    }
}