using System.Collections.Generic;

namespace PipelineExtensionLibrary
{
    public class LanguageFile
    {
        public Dictionary<string, LanuageLine> Translations { get; }

        public LanguageFile(Dictionary<string, LanuageLine> translations)
        {
            this.Translations = translations;
        }
    }

    public class LanuageLine
    {
        public Dictionary<Language, string> Translations { get; }

    }
}