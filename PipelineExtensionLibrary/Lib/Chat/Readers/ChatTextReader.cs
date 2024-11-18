using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace PipelineExtensionLibrary.Chat.Readers;

public class ChatTextReader : IComponentReader
{
    public int Id => 1;

    public bool Supports(IChatComponentData data)
    {
        return data is ChatTextData;
    }

    public IChatComponentData Read(ContentReader input, List<IComponentReader> readers)
    {
        var color = input.ReadColor();
        var text = input.ReadString();

        return new ChatTextData(color, text);
    }
}