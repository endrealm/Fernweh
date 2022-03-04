using System.Collections.Generic;
using PipelineExtensionLibrary.Chat;

namespace PipelineExtensionLibrary
{
    public class DialogTranslationData
    {
        public DialogTranslationData(Dictionary<string, DialogTranslationGroup> translationGroups)
        {
            TranslationGroups = translationGroups;
        }

        public Dictionary<string, DialogTranslationGroup> TranslationGroups { get; }
    }


    public class DialogTranslationGroup
    {
        public DialogTranslationGroup(Dictionary<Language, IChatComponentData> translatedComponents)
        {
            TranslatedComponents = translatedComponents;
        }

        public Dictionary<Language, IChatComponentData> TranslatedComponents { get; }
    }
}