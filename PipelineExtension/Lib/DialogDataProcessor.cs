using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using PipelineExtensionLibrary;
using TInput = System.String;
using TOutput = System.String;

namespace PipelineExtension
{
    [ContentProcessor(DisplayName = "DialogProcessor")]
    public class DialogDataProcessor : ContentProcessor<LanguageFile, DialogTranslationData>
    {
        public override DialogTranslationData Process(LanguageFile input, ContentProcessorContext context)
        {
            var parser = new TranslationTextParser();
            var data = new Dictionary<string, DialogTranslationGroup>();
            foreach (var entry in input.Translations)
            {
                var translated = entry.Value.Translations.
                    ToDictionary(
                        translatedLine => translatedLine.Key, 
                        translatedLine => parser.Parse(translatedLine.Value)
                    );
                data.Add(entry.Key, new DialogTranslationGroup(translated));
            }
            return new DialogTranslationData(data);
        }
    }
}