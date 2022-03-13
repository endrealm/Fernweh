using System;
using System.Collections.Generic;

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
}