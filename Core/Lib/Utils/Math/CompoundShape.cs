using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Utils.Math;

public class CompoundShape : IShape
{
    public CompoundShape(List<IShape> subShapes)
    {
        SubShapes = subShapes;
    }

    public List<IShape> SubShapes { get; }

    public bool IsInside(Vector2 point)
    {
        return SubShapes.Any(shape => shape.IsInside(point));
    }

    public IShape WithOffset(Vector2 offset)
    {
        return new CompoundShape(SubShapes.Select(shape => shape.WithOffset(offset)).ToList());
    }

    public void DebugDraw(SpriteBatch spriteBatch, Color color)
    {
        SubShapes.ForEach(shape => shape.DebugDraw(spriteBatch, color));
    }
}