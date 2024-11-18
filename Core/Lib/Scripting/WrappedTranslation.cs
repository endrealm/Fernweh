using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scripting;

public class WrappedTranslation
{
    public WrappedTranslation(ChatWrapper content)
    {
        Content = content;
    }

    // Do not document or use from lua
    public ChatWrapper Content { get; }
}