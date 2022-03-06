using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary.Chat;

namespace Core.Scenes.Ingame.Chat;

public static class DialogTranslationDataExtensions
{
    public static IChatComponent Build(this IChatComponentData data, SpriteFont font)
    {
        switch (data)
        {
            case ChatCompoundData compound:
            {
                var list = compound.Components.Select(componentData => componentData.Build(font) as IChatInlineComponent).ToList();
                return new CompoundTextComponent(list);
            }
            case ChatTextData text:
                return new TextComponent(font, text.Text, text.Color);
            default:
                return null;
        }
    }
}