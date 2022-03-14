using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace PipelineExtension;

[ContentImporter(".lua", DisplayName = "LuaImporter", DefaultProcessor = "LuaScriptProcessor")]
public class LuaScriptImporter : ContentImporter<string>
{
    public override string Import(string filename, ContentImporterContext context)
    {
        try
        {
            return File.ReadAllText(Path.GetFullPath(filename));
        }
        catch (Exception e)
        {
            context.Logger.LogImportantMessage(e.StackTrace);
            throw;
        }
    }
}