using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Saving.Impl;

public class BasicGameSave : IGameSave
{
    private readonly string _path;

    public BasicGameSave(string name, string path)
    {
        Name = name;
        _path = path;
    }

    public void Save()
    {
        File.WriteAllText(_path, JsonConvert.SerializeObject(Data));
    }

    public void Load()
    {
        var data = File.ReadAllText(_path);
        var raw = JObject.Parse(data);
        Data = new Dictionary<string, object>();

        foreach (var jToken in raw) Data.Add(jToken.Key, ParseData(jToken.Value));
    }

    public Dictionary<string, object> Data { get; set; } = new();

    public string Name { get; }

    private object ParseData(JToken jToken)
    {
        if (jToken is JArray array) return array.Select(ParseData).ToList();

        if (jToken is JObject obj)
        {
            var fullObject = new Dictionary<string, object>();

            foreach (var token in obj) fullObject.Add(token.Key, ParseData(token.Value));

            return fullObject;
        }

        return ((JValue) jToken).Value;
    }
}