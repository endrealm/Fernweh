﻿using Microsoft.Xna.Framework;

namespace Core.States;

public class NullState: IState
{
    private Color _defaultBackgroundColor;
    public string Id => "null";
    public void Render(StateRenderer renderer, RenderContext context)
    {
        // Do nothing
        renderer.SetBackgroundColor(_defaultBackgroundColor);
    }

    public bool ShowExit => false;
    public bool AllowMove => true;

    public void SetBackground(Color defaultBackgroundColor)
    {
        _defaultBackgroundColor = defaultBackgroundColor;
    }
}