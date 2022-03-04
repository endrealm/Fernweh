using Microsoft.Xna.Framework.Content.Pipeline;
using PipelineExtensionLibrary;
using TInput = System.String;
using TOutput = System.String;

namespace PipelineExtension
{
    [ContentProcessor(DisplayName = "DialogProcessor")]
    class DialogDataProcessor : ContentProcessor<LanguageFile, DialogTranslationData>
    {
        public override DialogTranslationData Process(LanguageFile input, ContentProcessorContext context)
        {
            return default(DialogTranslationData);
        }
    }
}