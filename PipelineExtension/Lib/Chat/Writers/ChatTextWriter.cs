using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using PipelineExtensionLibrary.Chat;

namespace PipelineExtension.Chat.Writers;

public class ChatTextWriter: IComponentWriter
{
    public int Id => 1;
    public bool Supports(IChatComponentData data)
    {
        return data is ChatTextData;
    }

    public void Write(IChatComponentData rawData, ContentWriter output, List<IComponentWriter> writers)
    {
        var data = (ChatTextData) rawData;
        output.Write(data.Color);
        output.Write(data.Text);
    }
}