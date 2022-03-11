using Microsoft.Xna.Framework.Content.Pipeline;
using TInput = System.String;
using TOutput = System.String;

namespace PipelineExtension
{
    [ContentProcessor(DisplayName = "LuaScriptProcessor")]
    public class LuaScriptProcessor : ContentProcessor<string, string>
    {
        public override string Process(string input, ContentProcessorContext context)
        {
            return input;
        }
    }
}