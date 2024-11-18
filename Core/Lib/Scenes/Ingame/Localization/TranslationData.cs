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
        if (Translations.TryGetValue(key, out var value)) return value;

        return new TranslatedItem(key, new Dictionary<Language, string>());
    }

    public void Merge(TranslationData translationData)
    {
        foreach (var pair in translationData.Translations)
            if (Translations.ContainsKey(pair.Key))
                Translations[pair.Key].Merge(pair.Value);
            else
                Translations.Add(pair.Key, pair.Value);
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
        if (Translations.TryGetValue(language, out var value)) return value;

        if (Translations.TryGetValue(fallback, out value)) return value;

        // Fallback to key
        return $"<color=\"Red\">{_key}</color>";
    }

    public void Merge(TranslatedItem other)
    {
        foreach (var pair in other.Translations)
            if (Translations.ContainsKey(pair.Key))
                Translations[pair.Key] = pair.Value; // Overwrite if exists!
            else
                Translations.Add(pair.Key, pair.Value);
    }
}