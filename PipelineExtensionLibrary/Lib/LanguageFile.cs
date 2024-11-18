using System.Collections.Generic;
using Newtonsoft.Json;

namespace PipelineExtensionLibrary;

public class LanguageFile
{
    public LanguageFile()
    {
    }

    public LanguageFile(Dictionary<string, LanuageLine> translations)
    {
        Translations = translations;
    }

    [JsonProperty("translations")]
    public Dictionary<string, LanuageLine> Translations { get; set; }
}

public class LanuageLine
{
    [JsonProperty("translations")]
    public Dictionary<Language, string> Translations { get; set; }
}