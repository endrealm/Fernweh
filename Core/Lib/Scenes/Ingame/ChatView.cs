using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Chat.Effects.Default;
using Core.States;
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
    private readonly StateManager _stateManager = new();
    private Queue<IChatComponent> _queuedComponents = new();
    private List<IChatComponent> _runningComponents = new();
    private int _width = 0;

    private int _xMargin = 5;
    private DialogTranslationData _translationData;

    public void Load(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/TinyUnicode");
        _translationData = content.Load<DialogTranslationData>("Dialogs/test");
        _stateManager.LoadScript( content.Load<string>("States/test"));
        var state = _stateManager.ReadState("my_state");
        RenderState(state);
        // _queuedComponents.Enqueue(new CompoundTextComponent((comp) => new List<IChatInlineComponent>
        // {
        // new TextComponent(_font,"This is an example message. That should automatically break at the end of the line. ", Color.White,
        //     contentEffect: new TypeWriterContentEffect(timePerParagraph: 0, onFinish: () =>
        //     {
        //         comp.AppendComponent(new TextComponent(_font, "Oh a second message.  ", Color.Gold, contentEffect: new TypeWriterContentEffect(timePerParagraph: 0,
        //         onFinish: () =>
        //         {
        //             comp.AppendComponent(new TextComponent(_font, "And a third message", Color.Green, contentEffect: new TypeWriterContentEffect(onFinish: ()=>
        //             {
        //                 LoadNextComponentInQueue();
        //             })));
        //         })));
        //     })
        // ),
        // }));
        // _queuedComponents.Enqueue(new CompoundTextComponent(new List<IChatInlineComponent>()
        // {
        //     new TextComponent(_font, "This is a ", Color.White),
        //     new TextComponent(_font, "second ", Color.Orange),
        //     new TextComponent(_font, "paragraph!", Color.White, contentEffect: new StaticContentEffect(onFinish: ()=>
        //     {
        //         LoadNextComponentInQueue();
        //     }))
        // }));
        // _queuedComponents.Enqueue(data.TranslationGroups["dialog.example"].TranslatedComponents[Language.EN_US].Build(_font));
        //
        // LoadNextComponentInQueue();
    }

    private void RenderState(IState state)
    {
        var renderer = new StateRenderer(_translationData, Language.EN_US, _font);
        state.Render(renderer, new RenderContext());
        _queuedComponents = renderer.Build();
        _runningComponents.Clear();
        LoadNextComponentInQueue();
    }

    private void LoadNextComponentInQueue()
    {
        if(_queuedComponents.Count == 0) return;
        var item = _queuedComponents.Dequeue();
        _runningComponents.Add(item);
        item.SetOnDone(LoadNextComponentInQueue);
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