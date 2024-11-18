using Core.Scenes.Ingame.Chat;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Views;

public interface ILabel : IRenderer<LabelRenderContext>
{
}

public struct LabelRenderContext : IRenderContext
{
    public LabelRenderContext(Vector2 baseScreenSize, int chatWidth)
    {
        BaseScreenSize = baseScreenSize;
        ChatWidth = chatWidth;
    }

    public Vector2 BaseScreenSize { get; }
    public int ChatWidth { get; }
}

public class BasicLabel : ILabel
{
    private readonly IChatComponent _component;
    private readonly LabelSettings _labelSettings;

    public BasicLabel(LabelSettings labelSettings, SpriteFont font)
    {
        _labelSettings = labelSettings;
        _component = labelSettings.Wrapper.Compile().Build(font);
    }

    public void Render(SpriteBatch spriteBatch, LabelRenderContext context)
    {
        _component.Render(spriteBatch,
            new ChatRenderContext(new Vector2(context.ChatWidth + _labelSettings.X, _labelSettings.Y)));
    }
}

public struct LabelSettings
{
    public int X { get; }
    public int Y { get; }
    public ChatWrapper Wrapper { get; }

    public LabelSettings(int x, int y, ChatWrapper wrapper)
    {
        X = x;
        Y = y;
        Wrapper = wrapper;
    }
}