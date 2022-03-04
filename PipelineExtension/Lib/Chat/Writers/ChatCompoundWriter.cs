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

    public IChatComponentData Read(ContentReader input, List<IComponentReader> readers)
    {
        var count = input.ReadInt32();
        var components = new List<IChatComponentData>();
        for (int i = 0; i < count; i++)
        {
            var readerId = input.ReadInt32();
            var selectedReader = readers.Find(writer => writer.Id == readerId);
            if (selectedReader == null)
            {
                throw new Exception("No reader found for id " + readerId);
            }

            var component = selectedReader.Read(input, readers);
            components.Add(component);
        }

        return new ChatCompoundData(components);
    }
}