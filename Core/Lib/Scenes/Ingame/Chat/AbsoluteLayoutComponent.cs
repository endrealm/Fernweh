using System;
using System.Collections.Generic;
using Core.Utils.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public class AbsoluteLayoutComponent : IChatContainerComponent
{
    private readonly List<(Anchor, IChatComponent)> _components = new();


    public AbsoluteLayoutComponent(float maxWidth, float maxHeight)
    {
        MaxWidth = maxWidth;
        MaxHeight = maxHeight;
    }

    public void Render(SpriteBatch spriteBatch, ChatRenderContext context)
    {
        _components.ForEach(tuple =>
        {
            var (anchor, chatComponent) = tuple;

            chatComponent.Render(spriteBatch,
                new ChatRenderContext(GetAnchorPosition(context.Position, anchor, chatComponent)));
        });
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        _components.ForEach(tuple =>
        {
            var (anchor, chatComponent) = tuple;

            var ctx = new ChatUpdateContext(context.IngameUpdateContext,
                GetAnchorPosition(context.Position, anchor, chatComponent), context.ClickHandled);
            chatComponent.Update(deltaTime, ctx);
            context.ClickHandled = ctx.ClickHandled;
        });
    }

    public Vector2 Dimensions => new(MaxWidth, MaxHeight);
    public float MaxWidth { get; set; }
    public IShape Shape { get; }

    public void SetOnDone(Action action)
    {
        // TODO maybe use?
    }

    public float MaxHeight { get; set; }
    public float MaxContentWidth { get; set; }

    public void AppendComponents(List<IChatInlineComponent> chatInlineComponents)
    {
        chatInlineComponents.ForEach(component => AppendComponent(component));
    }

    public void AppendComponent(IChatInlineComponent chatInlineComponent)
    {
        AppendComponent(chatInlineComponent, new Anchor
        {
            X = 0,
            Y = 0,
            Alignment = AnchorAlignment.TopLeft
        });
    }

    private Vector2 GetAnchorPosition(Vector2 thisPos, Anchor anchor, IChatComponent component)
    {
        var anchorPos = new Vector2(anchor.X, anchor.Y);
        return anchor.Alignment switch
        {
            AnchorAlignment.TopLeft => anchorPos + thisPos,
            AnchorAlignment.TopRight => anchorPos + thisPos + new Vector2(MaxWidth - component.Dimensions.X, 0),
            AnchorAlignment.BottomLeft => anchorPos + thisPos + new Vector2(0, MaxHeight - component.Dimensions.Y),
            AnchorAlignment.BottomRight => anchorPos + thisPos +
                                           new Vector2(MaxWidth - component.Dimensions.X,
                                               MaxHeight - component.Dimensions.Y),
            AnchorAlignment.CenterLeft => anchorPos + thisPos +
                                          new Vector2(0, (MaxHeight - component.Dimensions.Y) / 2),
            AnchorAlignment.CenterRight => anchorPos + thisPos + new Vector2(MaxWidth - component.Dimensions.X,
                (MaxHeight - component.Dimensions.Y) / 2),
            AnchorAlignment.CenterTop => anchorPos + thisPos + new Vector2((MaxWidth - component.Dimensions.X) / 2, 0),
            AnchorAlignment.CenterBottom => anchorPos + thisPos + new Vector2((MaxWidth - component.Dimensions.X) / 2,
                MaxHeight - component.Dimensions.Y),
            _ => thisPos
        };
    }

    public void AppendComponent(IChatComponent chatComponent, Anchor anchor)
    {
        _components.Add((anchor, chatComponent));
    }
}

public struct Anchor
{
    public AnchorAlignment Alignment { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public enum AnchorAlignment
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    CenterLeft,
    CenterRight,
    CenterTop,
    CenterBottom
}