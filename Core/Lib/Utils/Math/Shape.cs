﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Utils.Math;

public interface IShape
{
    bool IsInside(Vector2 point);

    /// <summary>
    ///     Returns a cloned version with an offset
    /// </summary>
    /// <param name="offset">the offset zo add</param>
    /// <returns>a cloned shape</returns>
    IShape WithOffset(Vector2 offset);

    void DebugDraw(SpriteBatch spriteBatch, Color color);
}