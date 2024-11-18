using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary.Chat;

namespace PipelineExtensionLibrary;

public class DialogTranslationData
{
    public DialogTranslationData(Dictionary<string, DialogTranslationGroup> translationGroups)
    {
        TranslationGroups = translationGroups;
    }

    public Dictionary<string, DialogTranslationGroup> TranslationGroups { get; }

    public IChatComponentData GetOrKey(string key, Language language = Language.EN_US)
    {
        IChatComponentData text;
        if (!TranslationGroups.ContainsKey(key) ||
            !TranslationGroups[key].TranslatedComponents.ContainsKey(Language.EN_US))
            // Fallback to key
            text = new ChatCompoundData(new List<IChatComponentData>
            {
                new ChatTextData(Color.Red, key)
            });
        else
            // select actual translation
            text = TranslationGroups[key].TranslatedComponents[Language.EN_US];

        return text;
    }
}

public class DialogTranslationGroup
{
    public DialogTranslationGroup(Dictionary<Language, IChatComponentData> translatedComponents)
    {
        TranslatedComponents = translatedComponents;
    }

    public Dictionary<Language, IChatComponentData> TranslatedComponents { get; }
}