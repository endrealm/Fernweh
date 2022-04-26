using System.Collections.Generic;
using Core.Content;
using Core.Scenes.Modding;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Localization;

public interface ILocalizationManager
{
    public ChatWrapper GetData(string key, params IReplacement[] replacements);
    void LoadLangs(List<Mod> mods, ContentLoader loader);
}