using System;
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
    private Queue<IChatComponent> _queuedComponents = new();
    private List<IChatComponent> _runningComponents = new();
    private int _width = 0;

    private int _xMargin = 5;
    
    public void Load(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/TinyUnicode");
        var data = content.Load<DialogTranslationData>("Dialogs/test");

        _queuedComponents.Enqueue(new CompoundTextComponent((comp) => new List<IChatInlineComponent>
        {
        new TextComponent(_font,"This is an example message. That should automatically break at the end of the line. ", Color.White,
            contentEffect: new TypeWriterContentEffect(timePerParagraph: 0, onFinish: () =>
            {
                comp.AppendComponent(new TextComponent(_font, "Oh a second message.  ", Color.Gold, contentEffect: new TypeWriterContentEffect(timePerParagraph: 0,
                onFinish: () =>
                {
                    comp.AppendComponent(new TextComponent(_font, "And a third message", Color.Green, contentEffect: new TypeWriterContentEffect(onFinish: ()=>
                    {
                        LoadNextComponentInQueue();
                    })));
                })));
            })
        ),
        }));
        _queuedComponents.Enqueue(new CompoundTextComponent(new List<IChatInlineComponent>()
        {
            new TextComponent(_font, "This is a ", Color.White),
            new TextComponent(_font, "second ", Color.Orange),
            new TextComponent(_font, "paragraph!", Color.White, contentEffect: new StaticContentEffect(onFinish: ()=>
            {
                LoadNextComponentInQueue();
            }))
        }));
        _queuedComponents.Enqueue(data.TranslationGroups["dialog.example"].TranslatedComponents[Language.EN_US].Build(_font));

        LoadNextComponentInQueue();
    }

    private void LoadNextComponentInQueue()
    {
        _runningComponents.Add(_queuedComponents.Dequeue());
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // Do width auto resize
        if (context.ChatWidth != _width)
        {
            _width = context.ChatWidth;
            _runningComponents.ForEach(component =>
            {
                component.MaxWidth = _width - _xMargin * 2;
            });
        }
        // Draw UI here
        spriteBatch.FillRectangle(new Vector2(), new Size2(context.ChatWidth, context.BaseScreenSize.Y), context.BackgroundColor);
        var offsetY = 0f;
        _runningComponents.ForEach(component =>
        {
            component.Render(spriteBatch, new ChatRenderContext(new Vector2(_xMargin, offsetY)));
            offsetY += component.Dimensions.Y;
        });
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        List<IChatComponent>  _tempRunningComponents = new List<IChatComponent>(_runningComponents);
        _tempRunningComponents.ForEach(component =>
        {
            component.Update(deltaTime, new ChatUpdateContext());
        });
    }
}