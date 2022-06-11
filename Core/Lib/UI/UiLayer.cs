using System.Collections.Generic;
using Core.Input;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Localization;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.UI;

public class UiLayer: IUpdate<TopLevelUpdateContext>, IRenderer<TopLevelRenderContext>
{
    
    private readonly IChatContainerComponent _root;

    public UiLayer(IFontManager fontManager, ILocalizationManager rootLocalizationManager)
    {
        _root = new CompoundTextComponent(new List<IChatInlineComponent>()
        {
            // rootLocalizationManager.GetData("some.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
            // rootLocalizationManager.GetData("some.other.example.key.here").Compile().Build(fontManager.GetChatFont()),
        }, layout: CompoundLayoutRule.Center);
    }

    public void Update(float deltaTime, TopLevelUpdateContext context)
    {
        var clicked = InteractionHelper.CursorHandled;
        var ctx = new ChatUpdateContext(new IngameUpdateContext(context), new Vector2(0, 0), clicked);
        _root.Update(deltaTime, ctx);
        clicked = ctx.ClickHandled;
        InteractionHelper.CursorHandled = clicked;
    }

    public void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {

        if ((int) _root.MaxWidth != (int)context.BaseScreenSize.X)
        {
            _root.MaxWidth = context.BaseScreenSize.X;
            _root.MaxContentWidth = context.BaseScreenSize.X/2;
        }
        if ((int) _root.MaxHeight != (int)context.BaseScreenSize.Y)
        {
            _root.MaxHeight = context.BaseScreenSize.Y;
        }
        spriteBatch.Begin(
            transformMatrix: context.Camera.GetViewMatrix(new Vector2()),
            samplerState: SamplerState.PointClamp
        );
        _root.Render(spriteBatch, new ChatRenderContext(new Vector2(0, 0)));
        spriteBatch.End();
    }
}