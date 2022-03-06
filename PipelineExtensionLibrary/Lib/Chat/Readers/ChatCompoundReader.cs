using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace PipelineExtensionLibrary.Chat.Readers;

public class ChatCompoundReader: IComponentReader
{
    public int Id => 0;
    public bool Supports(IChatComponentData data)
    {
        return data is ChatCompoundData;
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