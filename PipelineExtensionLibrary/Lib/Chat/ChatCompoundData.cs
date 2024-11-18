using System.Collections.Generic;

namespace PipelineExtensionLibrary.Chat;

public class ChatCompoundData : IChatComponentData
{
    public ChatCompoundData(List<IChatComponentData> components)
    {
        Components = components;
    }

    public List<IChatComponentData> Components { get; }
}