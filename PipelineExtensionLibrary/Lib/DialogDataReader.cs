using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using PipelineExtensionLibrary.Chat;
using PipelineExtensionLibrary.Chat.Readers;

namespace PipelineExtensionLibrary
{
    public class DialogDataReader: ContentTypeReader<DialogTranslationData>
    {
        private readonly List<IComponentReader> _readers = new() {new ChatCompoundReader()};

        
        protected override DialogTranslationData Read(ContentReader input, DialogTranslationData existingInstance)
        {
            var translationGroups = new Dictionary<string, DialogTranslationGroup>();
            
            var count = input.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var translatedComponents = new Dictionary<Language, IChatComponentData>();
                var key = input.ReadString();
                var componentCount = input.ReadInt32();
                for (int j = 0; j < componentCount; j++)
                {
                    var lang = (Language) input.ReadInt32();
                    var readerId = input.ReadInt32();
                    var selectedReader = _readers.Find(writer => writer.Id == readerId);
                    if (selectedReader == null)
                    {
                        throw new Exception("No reader found for id " + readerId);
                    }

                    var component = selectedReader.Read(input, _readers);
                    translatedComponents.Add(lang, component);
                }
                
                translationGroups.Add(key, new DialogTranslationGroup(translatedComponents));
            }

            return new DialogTranslationData(translationGroups);
        }
    }
}