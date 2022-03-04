using System.Collections.Generic;
using System.Linq;
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
            var data = new Dictionary<string, DialogTranslationGroup>();
            
            foreach (var entry in input.Translations)
            {
                var translated = entry.Value.Translations.
                    ToDictionary(
                        translatedLine => translatedLine.Key, 
                        translatedLine => XmlDialogParser.Parse(translatedLine.Value)
                    );
                data.Add(entry.Key, new DialogTranslationGroup(translated));
            }
            return new DialogTranslationData(data);
        }
    }
}