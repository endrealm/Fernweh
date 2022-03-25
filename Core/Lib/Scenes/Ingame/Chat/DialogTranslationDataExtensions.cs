using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Chat.Effects.Default;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary.Chat;

namespace Core.Scenes.Ingame.Chat;

public static class DialogTranslationDataExtensions
{
    public static IChatComponent Build(this IChatComponentData data, SpriteFont font, params Replacement[] replacements)
    {
        switch (data)
        {
            case ChatCompoundData compound:
            {
                var list = compound.Components.Select(componentData => componentData.Build(font, replacements) as IChatInlineComponent).ToList();
                return new CompoundTextComponent(list);
            }
            case ChatTextData text:
                return new TextComponent(font, text.Text.WithReplacements(replacements), text.Color);
            default:
                return null;
        }
    }

    private static string WithReplacements(this string value, IEnumerable<Replacement> replacements)
    {
        foreach (var (key, replaceValue) in replacements)
        {
            value = value.Replace("{"+key+ "}", replaceValue);
        }

        return value;
    }



    public static IChatComponent BuildAnimated(this IChatComponentData data, SpriteFont font, Action onFinish, params Replacement[] replacements)
    {
        return data.BuildAnimatedInternal(font, replacements, onFinish:onFinish);
    }
    public static IChatComponent BuildAnimatedAction(this IChatComponentData data, SpriteFont font, Action onClick, params Replacement[] replacements)
    {
        return data.BuildAnimatedInternal(font, replacements,true, onClick);
    }
    private static IChatComponent BuildAnimatedInternal(
        this IChatComponentData data,
        SpriteFont font,
        Replacement[] replacements,
        bool clickable = false,
        Action onClick = null,
        Action onFinish = null
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
                    var component = componentData.BuildAnimated(font, null, replacements) as IChatInlineComponent;
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
                return new TextComponent(font, text.Text.WithReplacements(replacements), text.Color, contentEffect: new TypeWriterContentEffect(timePerParagraph: 0));
            default:
                return null;
        }
    }
}

public readonly record struct Replacement(string Key, string Value)
{
    public string Key { get; } = Key;
    public string Value { get; } = Value;

}