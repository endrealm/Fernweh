using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace PipelineExtensionLibrary.Chat;

public interface IComponentReader
{
    int Id { get; }
    bool Supports(IChatComponentData data);
    IChatComponentData Read(ContentReader input, List<IComponentReader> readers);
}