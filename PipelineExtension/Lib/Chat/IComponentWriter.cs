using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using PipelineExtensionLibrary.Chat;

namespace PipelineExtension.Chat;

public interface IComponentWriter
{
    int Id { get; }
    bool Supports(IChatComponentData data);
    void Write(IChatComponentData data, ContentWriter output, List<IComponentWriter> writers);
}