using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Core.Utils.Math;

public class CompoundShape: IShape
{
    public List<IShape> SubShapes { get; }

    public CompoundShape(List<IShape> subShapes)
    {
        SubShapes = subShapes;
    }

    public bool IsInside(Vector2 point)
    {
        return SubShapes.Any(shape => shape.IsInside(point));
    }

    public IShape WithOffset(Vector2 offset)
    {
        return new CompoundShape(SubShapes.Select(shape => shape.WithOffset(offset)).ToList());
    }
}