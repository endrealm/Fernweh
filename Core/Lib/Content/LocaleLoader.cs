using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Localization;
using Newtonsoft.Json.Linq;
using PipelineExtensionLibrary;

namespace Core.Content;

public class LocaleLoader : ILoader<TranslationData>
{
    public TranslationData Load(string file, IArchiveLoader archiveLoader)
    {
        var text = archiveLoader.LoadFile(file);
        var jObject = JObject.Parse(text);

        // possibly check for format version

        var data = jObject.ToObject<Dictionary<string, LanuageLine>>();
        var dict = data!.ToDictionary(pair => pair.Key, pair => pair.Value.ToTranslatedItem(pair.Key));
        return new TranslationData(dict);
    }
}

public static class TranslationExt
{
    public static TranslatedItem ToTranslatedItem(this LanuageLine line, string key)
    {
        var dict = line.Translations.ToDictionary(pair => pair.Key, pair => pair.Value);
        return new TranslatedItem(key, dict);
    }
}