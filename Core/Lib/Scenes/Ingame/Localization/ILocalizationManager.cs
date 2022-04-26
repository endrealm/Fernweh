using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Localization;

public interface ILocalizationManager
{
    public TranslationData TranslationData { set; }
    public ChatWrapper GetData(string key, params IReplacement[] replacements);
}