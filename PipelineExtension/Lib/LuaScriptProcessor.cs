using Microsoft.Xna.Framework.Content.Pipeline;

namespace PipelineExtension;

[ContentProcessor(DisplayName = "LuaScriptProcessor")]
public class LuaScriptProcessor : ContentProcessor<string, string>
{
    public override string Process(string input, ContentProcessorContext context)
    {
        return input;
    }
}