namespace PipelineExtensionLibrary.Tokenizer.Chat;

public interface IReplacement
{
    public string Key { get; }
    public ChatWrapper ChatWrapper { get; }
}

public readonly record struct TextReplacement(string Key, string Value): IReplacement
{
    public string Key { get; } = Key;
    public string Value { get; } = Value;
    public ChatWrapper ChatWrapper => new TextWrapper(Value);

}

public readonly record struct WrapperReplacement(string Key, ChatWrapper Value): IReplacement
{
    public string Key { get; } = Key;
    public ChatWrapper Value { get; } = Value;
    public ChatWrapper ChatWrapper => Value;

}