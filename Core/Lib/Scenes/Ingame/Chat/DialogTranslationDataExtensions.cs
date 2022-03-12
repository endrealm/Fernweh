using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Chat.Effects.Default;
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
    
    public static IChatComponent BuildAnimated(this IChatComponentData data, SpriteFont font)
    {
        switch (data)
        {
            case ChatCompoundData compound:
            {
                var list = new List<IChatInlineComponent>();
                var queue = new Queue<IChatInlineComponent>();
                var compoundElement = new CompoundTextComponent(list);
                compound.Components.ForEach(componentData =>
                {
                    var component = componentData.BuildAnimated(font) as IChatInlineComponent;
                    queue.Enqueue(component);
                    component!.SetOnDone(() =>
                    {
                        if (queue.Count > 0)
                        {
                            compoundElement.AppendComponent(queue.Dequeue());
                        }
                        else
                        {
                            compoundElement.Done();
                        }
                    });
                });
                if (queue.Count > 0)
                {
                    list.Add(queue.Dequeue());
                }
                else
                {
                    compoundElement.Done();
                }
                return compoundElement;
            }
            case ChatTextData text:
                return new TextComponent(font, text.Text, text.Color, contentEffect: new TypeWriterContentEffect(timePerParagraph: 0));
            default:
                return null;
        }
    }
}