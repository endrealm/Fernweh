using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using PipelineExtensionLibrary;

namespace PipelineExtension;

[ContentImporter(".lang", DisplayName = "LanguageFileImporter", DefaultProcessor = "DialogProcessor")]
public class DialogDataImporter : ContentImporter<LanguageFile>
{
    public override LanguageFile Import(string filename, ContentImporterContext context)
    {
        try
        {
            var content = File.ReadAllText(Path.GetFullPath(filename));
            var data = JsonConvert.DeserializeObject<Dictionary<string, LanuageLine>>(content);

            return new LanguageFile(data);
        }
        catch (Exception e)
        {
            context.Logger.LogImportantMessage(e.StackTrace);
            throw;
        }
    }
}