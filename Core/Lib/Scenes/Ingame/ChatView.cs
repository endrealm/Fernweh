﻿using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Chat.Effects.Default;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame;

public class ChatView: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    private SpriteFont _font;
    private List<IChatComponent> _components = new();
    private int _width = 0;
    
    
    public void Load(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/TinyUnicode");
        var data = content.Load<DialogTranslationData>("Dialogs/test");
        Console.WriteLine(data);
        _components = new List<IChatComponent>
        {
            new CompoundTextComponent((comp) => new List<IChatInlineComponent>
            {
                new TextComponent(_font,"This is an example message. That should automatically break at the end of the line.", Color.White, 
                    contentEffect: new TypeWriterContentEffect(onFinish: () =>
                    {
                        comp.AppendComponent(new TextComponent(_font, "Oh a second message", Color.Gold, contentEffect: new TypeWriterContentEffect(
                            onFinish: () =>
                            {
                                comp.AppendComponent(new TextComponent(_font, "And a third message", Color.Green, contentEffect: new TypeWriterContentEffect()));
                            }
                        )));
                    })
                ),
            }),
            new TextComponent(_font, "This is a new paragraph", Color.Green),
            new CompoundTextComponent(new List<IChatInlineComponent>()
            {
                new TextComponent(_font, "This is a ", Color.White),
                new TextComponent(_font, "third ", Color.Orange),
                new TextComponent(_font, "paragraph!", Color.White)
            })
        };
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // Do width auto resize
        if (context.ChatWidth != _width)
        {
            _width = context.ChatWidth;
            _components.ForEach(component =>
            {
                component.MaxWidth = _width;
            });
        }
        // Draw UI here
        spriteBatch.FillRectangle(new Vector2(), new Size2(context.ChatWidth, context.BaseScreenSize.Y), context.BackgroundColor);
        var offsetY = 0f;
        _components.ForEach(component =>
        {
            component.Render(spriteBatch, new ChatRenderContext(new Vector2(0, offsetY)));
            offsetY += component.Dimensions.Y;
        });
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        _components.ForEach(component =>
        {
            component.Update(deltaTime, new ChatUpdateContext());
        });
    }
}