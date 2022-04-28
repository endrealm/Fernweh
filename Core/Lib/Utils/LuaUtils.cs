using System;
using System.Collections.Generic;
using Core.Scripting;
using NLua;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Utils;

public sealed class LuaUtils
{
    public static IReplacement[] ReadReplacements(LuaTable rawReplacements)
    {
        if (rawReplacements == null) return Array.Empty<IReplacement>();
        var list = new List<IReplacement>();
        foreach (var replacement in rawReplacements.Values)
        {
            if (!(replacement is LuaTable table)) throw new Exception("Invalid replacement parameter");
            var values = table.Values.GetEnumerator();
            values.MoveNext();
            var key = values.Current;
            values.MoveNext();
            var value = values.Current;

            switch (value)
            {
                case WrappedTranslation translation:
                    list.Add(new WrapperReplacement(key!.ToString(), translation.Content));
                    break;
                case ChatWrapper wrapper:
                    list.Add(new WrapperReplacement(key!.ToString(), wrapper));
                    break;
                default:
                    list.Add(new TextReplacement(key!.ToString(), value!.ToString()));
                    break;
            }
        }

        return list.ToArray();
    }
}