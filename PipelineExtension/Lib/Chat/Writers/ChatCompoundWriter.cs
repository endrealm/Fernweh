using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using PipelineExtensionLibrary.Chat;

namespace PipelineExtension.Chat.Writers;

public class ChatCompoundWriter: IComponentWriter
{
    public int Id => 0;
    public bool Supports(IChatComponentData data)
    {
        return data is ChatCompoundData;
    }

    public void Write(IChatComponentData rawData, ContentWriter output, List<IComponentWriter> writers)
    {
        var data = (ChatCompoundData) rawData;
        output.Write(data.Components.Count);
        foreach (var componentData in data.Components)
        {
            var selectedWriter = writers.Find(writer => writer.Supports(componentData));
            if (selectedWriter == null)
            {
                throw new Exception("No writer found for component " + componentData);
            }
            output.Write(selectedWriter.Id);
            selectedWriter.Write(componentData, output, writers);
        }
    }
}