using Microsoft.Xna.Framework.Content;

namespace Core.Utils;

public interface ILoadable
{
    void Load(ContentManager content);
}

public interface ILoadable<T>
{
    void Load(ContentManager content, T context);
}