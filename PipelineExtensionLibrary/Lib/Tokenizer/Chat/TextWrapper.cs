using System;
using System.Collections.Generic;

namespace PipelineExtensionLibrary.Tokenizer.Chat;

public class TextWrapper : ChatWrapper
{
    private bool _noText;

    public TextWrapper(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public override List<MergeResult> Flatten(MergeResult parent)
    {
        if (_noText) return base.Flatten(parent);

        var children = base.Flatten(parent);

        children.Insert(0, Merge(parent));

        return children;
    }

    public override void Replace(IReplacement[] replacements)
    {
        if (_noText)
        {
            base.Replace(replacements);
            return;
        }

        foreach (var replacement in replacements)
        {
            if (!Text.Contains("{" + replacement.Key + "}")) continue;

            var parts = Text.Split(new[] {"{" + replacement.Key + "}"}, StringSplitOptions.None);
            _noText = true;
            var last = parts.Length - 1;
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (part.Length > 0) AddChild(new TextWrapper(part));
                if (i >= last) continue;
                AddChild(replacement.ChatWrapper.Clone());
            }

            break;
        }

        base.Replace(replacements);
    }

    public override MergeResult Merge(MergeResult parent)
    {
        if (_noText) return base.Merge(parent);
        var merge = parent.Clone();
        merge.Text = Text;
        return merge;
    }

    public override ChatWrapper Clone()
    {
        if (_noText) return base.Clone();

        var wrapper = new TextWrapper(Text);
        CloneInto(wrapper);
        return wrapper;
    }
}