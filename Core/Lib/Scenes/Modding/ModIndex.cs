using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Scenes.Modding;

public class ModIndex
{
    public string[] Dependencies = Array.Empty<string>();

    public string Id;
    public string[] Scripts = Array.Empty<string>();

    [JsonConverter(typeof(StringEnumConverter))]
    public ModType type;
}

public enum ModType
{
    Library,
    Game
}