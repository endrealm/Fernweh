using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scripting;

public class WrappedTranslation
{
    
    // Do not document or use from lua
    public ChatWrapper Content { get; }
    
    public WrappedTranslation(ChatWrapper content)
    {
        Content = content;
    }
}