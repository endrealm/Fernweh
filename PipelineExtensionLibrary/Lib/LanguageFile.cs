using System.Collections.Generic;
using Newtonsoft.Json;

namespace PipelineExtensionLibrary
{
    public class LanguageFile
    {
        [JsonProperty("translations")]
        public Dictionary<string, LanuageLine> Translations { get; set; }

        public LanguageFile() {}
        
        public LanguageFile(Dictionary<string, LanuageLine> translations)
        {
            this.Translations = translations;
        }
    }

    public class LanuageLine
    {
        [JsonProperty("translations")]
        public Dictionary<Language, string> Translations { get; set; }

    }
}