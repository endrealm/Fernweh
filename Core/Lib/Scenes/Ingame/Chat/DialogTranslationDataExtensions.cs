using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Chat.Effects.Default;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary.Chat;
using PipelineExtensionLibrary.Tokenizer.Chat;

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

    public static IChatComponentData Compile(this ChatWrapper wrapper)
    {
        var data = wrapper.Flatten(new MergeResult
        {
            Color = Color.White
        }).Select(result => new ChatTextData(result.Color, result.Text)).ToList<IChatComponentData>();
        
        return new ChatCompoundData(data);
    }

    public static IChatComponent BuildAnimated(this IChatComponentData data, SpriteFont font, Action onFinish, bool animated = true)
    {
        return data.BuildAnimatedInternal(font, onFinish: onFinish, animated: animated);
    }
    public static IChatComponent BuildAnimatedAction(this IChatComponentData data, SpriteFont font, Action onClick, bool animated = true)
    {
        return data.BuildAnimatedInternal(font,true, onClick, animated: animated);
    }
    private static IChatComponent BuildAnimatedInternal(
        this IChatComponentData data,
        SpriteFont font,
        bool clickable = false,
        Action onClick = null,
        Action onFinish = null,
        bool animated = true
    ) {
        switch (data)
        {
            case ChatCompoundData compound:
            {
                var list = new List<IChatInlineComponent>();
                var queue = new Queue<IChatInlineComponent>();
                var compoundElement = clickable ? new ActionButtonComponent(onClick, list) : new CompoundTextComponent(list);
                compound.Components.ForEach(componentData =>
                {
                    var component = componentData.BuildAnimated(font, null) as IChatInlineComponent;
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
                            onFinish?.Invoke();
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
                    onFinish?.Invoke();
                }
                return compoundElement;
            }
            case ChatTextData text:
                return new TextComponent(font, text.Text, text.Color, contentEffect: animated ? new TypeWriterContentEffect(timePerParagraph: 0) : new StaticContentEffect(timerPerParagraph: 0));
            default:
                return null;
        }
    }
}