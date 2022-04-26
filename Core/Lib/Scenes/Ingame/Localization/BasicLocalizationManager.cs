using System;
using System.Collections.Generic;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Localization;

public class BasicLocalizationManager: ILocalizationManager
{

    private Language _language = Language.EN_US;
    public TranslationData TranslationData { private get; set; }
    private TranslationTextParser _parser = new();
    

    public BasicLocalizationManager()
    {
        TranslationData = new TranslationData(new Dictionary<string, TranslatedItem>());
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

    private TranslatedItem GetRawTranslation(string key)
    {
        return TranslationData.Get(key);
    }
}