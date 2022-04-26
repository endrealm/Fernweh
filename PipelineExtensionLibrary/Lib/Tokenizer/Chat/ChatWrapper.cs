using System.Collections.Generic;
using JetBrains.Annotations;

namespace PipelineExtensionLibrary.Tokenizer.Chat;

public class ChatWrapper
{
    private readonly List<ChatWrapper> _wrappers = new();
    public ChatWrapper AddChild(ChatWrapper wrapper)
    {
        _wrappers.Add(wrapper);
        return this;
    }

    public virtual List<MergeResult> Flatten(MergeResult parent)
    {
        var items = new List<MergeResult>();

        var merged = Merge(parent);
        _wrappers.ForEach(wrapper =>
        {
            items.AddRange(wrapper.Flatten(merged));
        });

        return items;
    }
    
    public virtual MergeResult Merge(MergeResult parent)
    {
        return parent;
    }

    public virtual void Replace(IReplacement[] replacements)
    {
        _wrappers.ForEach(wrapper =>
        {
            wrapper.Replace(replacements);
        });

    }

    public virtual ChatWrapper Clone()
    {
        var wrapper = new ChatWrapper();
        CloneInto(wrapper);
        return wrapper;
    }

    protected void CloneInto(ChatWrapper wrapper)
    {
        foreach (var chatWrapper in _wrappers)
        {
            wrapper.AddChild(chatWrapper.Clone());
        }
    }

}