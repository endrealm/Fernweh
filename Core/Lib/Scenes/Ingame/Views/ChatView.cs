using System;
using Core.Scenes.Ingame.Chat;
using Core.Utils;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Views;

public interface IChatView : IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    public IChatComponent AddText(string key, Action callback = null, params IReplacement[] replacements);
    public IChatComponent AddText(string key, params IReplacement[] replacements);
    public IChatComponent AddAction(string key, Action callback, params IReplacement[] replacements);
    void ForceLoadNext();
    void Clear();
    void SetSticky(bool sticky);
    public ILabel DrawLabel(LabelSettings settings);
}