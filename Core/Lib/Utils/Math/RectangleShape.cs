﻿using Microsoft.Xna.Framework;

namespace Core.Utils.Math;

public class RectangleShape: IShape
{
    public float Width { get; set; }
    public float Height { get; set; }
    public Vector2 Position { get; set; }

    public RectangleShape(Vector2 position, float width, float height)
    {
        Position = position;
        Width = width;
        Height = height;
    }

    public bool IsInside(Vector2 point)
    {
        return point.X >= Position.X && point.Y >= Position.Y && point.X <= (Position.X+Width) && point.Y <= (Position.Y + Height);
    }

    public IShape WithOffset(Vector2 offset)
    {
        return new RectangleShape(Position + offset, Width, Height);
    }
}