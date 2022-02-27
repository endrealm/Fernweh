using Microsoft.Xna.Framework.Content;

namespace Core.Utils;

public interface ILoadable
{
    void Load(ContentManager content);
}