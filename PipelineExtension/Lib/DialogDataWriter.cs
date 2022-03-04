using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using PipelineExtension.Chat;
using PipelineExtension.Chat.Writers;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Chat;

namespace PipelineExtension
{
    public class DialogDataWriter: ContentTypeWriter<DialogTranslationData>
    {
        private readonly List<IComponentWriter> _writers = new() { new ChatCompoundWriter() };

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "PipelineExtensionLibrary.DialogDataReader";
        }

        protected override void Write(ContentWriter output, DialogTranslationData value)
        {
            var translationCount = value.TranslationGroups.Count;
            output.Write(translationCount);
            foreach (var entry in value.TranslationGroups)
            {
                var components = entry.Value.TranslatedComponents;
                output.Write(entry.Key);
                output.Write(components.Count);

                foreach (var translationEntry in components)
                {
                    output.Write((int) translationEntry.Key);
                    var selectedWriter = _writers.Find(writer => writer.Supports(translationEntry.Value));
                    if (selectedWriter == null)
                    {
                        throw new Exception("No writer found for component " + translationEntry.Value);
                    }
                    output.Write(selectedWriter.Id);
                    selectedWriter.Write(translationEntry.Value, output, _writers);
                }
            }
        }
    }
}