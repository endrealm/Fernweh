using System;
using System.Collections.Generic;
using System.IO;
using Core.Content;
using Core.Scenes.Modding;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Localization;

public class BasicLocalizationManager: ILocalizationManager
{

    private Language _language = Language.EN_US;
    private readonly TranslationData _translationData;
    private TranslationTextParser _parser = new();
    

    public BasicLocalizationManager()
    {
        _translationData = new TranslationData(new Dictionary<string, TranslatedItem>());
    }

    public ChatWrapper GetData(string key, params IReplacement[] replacements)
    {
        var item = GetRawTranslation(key);
        var text = item.Get(_language);
        ChatWrapper parsed;
        try
        {
            parsed = _parser.ParseWrapped(text);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            parsed = _parser.ParseWrapped("<color=\"Red\">parse error</color>");
        }

        parsed.Replace(replacements);
        
        return parsed;
    }

    public void LoadLangs(List<Mod> mods, ContentLoader contentLoader)
    {
        mods.ForEach(mod =>
        {
            var files = mod.Archive.LoadAllFiles("*.lang");
            foreach (var file in files)
            {
                var translationData = contentLoader.Load<TranslationData>(file, mod.Id);
                _translationData.Merge(translationData);
            }
        });
    }

    private TranslatedItem GetRawTranslation(string key)
    {
        return _translationData.Get(key);
    }
}