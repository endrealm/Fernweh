using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Scenes.Modding;

public class ModIndex
{

    public string Id;
    
    [JsonConverter(typeof(StringEnumConverter))]
    public ModType type;
    public string[] Dependencies = Array.Empty<string>();
    public string[] Scripts = Array.Empty<string>();

}

public enum ModType
{
    Library,
    Game
}
