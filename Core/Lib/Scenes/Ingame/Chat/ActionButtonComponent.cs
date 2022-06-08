using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame.Chat;

public class ActionButtonComponent: CompoundTextComponent, IAction
{
    private readonly Action _onClick;

    public ActionButtonComponent(
        Action onClick,
        List<IChatInlineComponent> components, 
        float width = -1,
        float maxWidth = -1
    ) : this(onClick, ct => components, width, maxWidth) { }

    public ActionButtonComponent(
        Action onClick,
        Func<CompoundTextComponent, List<IChatInlineComponent>> construct, 
        float width = -1, 
        float maxWidth = -1
    ) : base(construct, width, maxWidth)
    {
        _onClick = onClick;
    }

    public void OnClick()
    {
        _onClick.Invoke();
    }

    public override void Update(float deltaTime, ChatUpdateContext context)
    {
        base.Update(deltaTime, context);
        if(context.ClickHandled) return;
        var input = context.IngameUpdateContext.TopLevelUpdateContext.ClickInput;
        if(!input.ClickedThisFrame) return;
        var camera = context.IngameUpdateContext.TopLevelUpdateContext.Camera;
        var pos = Vector2.Transform(input.ScreenSpacedCoordinates, Matrix.Invert(camera.GetViewMatrix(new Vector2())));
        if (!Shape.WithOffset(context.Position).IsInside(pos)) return;
        context.ClickHandled = true;
        _onClick.Invoke();
    }
}