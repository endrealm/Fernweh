using System.Collections.Generic;
using Core.Input;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Chat;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.UI;

public class UiLayer: IUpdate<TopLevelUpdateContext>, IRenderer<TopLevelRenderContext>
{
    
    private readonly IChatContainerComponent _root;

    public UiLayer()
    {
        _root = new CompoundTextComponent(new List<IChatInlineComponent>());
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
        _root.Render(spriteBatch, new ChatRenderContext(new Vector2(0, 0)));
    }
}