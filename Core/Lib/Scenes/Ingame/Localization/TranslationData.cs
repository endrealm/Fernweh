using System.Collections.Generic;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Localization;

public class TranslationData
{
    public TranslationData(Dictionary<string, TranslatedItem> translations)
    {
        Translations = translations;
    }

    private Dictionary<string, TranslatedItem> Translations { get; }

    public TranslatedItem Get(string key)
    {
        if (Translations.TryGetValue(key, out var value))
        {
            return value;
        }

        return new TranslatedItem(key, new Dictionary<Language, string>());
    }
}

public class TranslatedItem
{
    private readonly string _key;

    public TranslatedItem(string key, Dictionary<Language, string> translations)
    {
        _key = key;
        Translations = translations;
    }

    private Dictionary<Language, string> Translations { get; }

    public string Get(Language language, Language fallback = Language.EN_US)
    {
        if (Translations.TryGetValue(language, out var value))
        {
            return value;
        }

        if (Translations.TryGetValue(fallback, out value))
        {
            return value;
        }
        
        // Fallback to key
        return $"<color=\"Red\">{_key}</color>";

    }
}