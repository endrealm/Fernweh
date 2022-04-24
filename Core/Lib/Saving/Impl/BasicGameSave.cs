using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Saving.Impl;

public class BasicGameSave: IGameSave
{
    private readonly string _path;
    private Dictionary<string, object> _data = new();

    public BasicGameSave(string path)
    {
        _path = path;
    }

    public void Save()
    {
        File.WriteAllText(_path, JsonConvert.SerializeObject(_data));
    }

    public void Load()
    {
        var data = File.ReadAllText(_path);
        var raw = JObject.Parse(data);
        _data = new();
        
        foreach (var jToken in raw)
        {
            _data.Add(jToken.Key, ParseData(jToken.Value));
        }
    }

    private object ParseData(JToken jToken)
    {
        if (jToken is JArray array)
        {
            return array.Select(ParseData).ToList();
        }
        
        if (jToken is JObject obj)
        {
            var fullObject = new Dictionary<string, object>();
            
            foreach (var token in obj)
            {
                fullObject.Add(token.Key, ParseData(token.Value));
            }
            
            return fullObject;
        }

        return ((JValue) jToken).Value;
    }

    public Dictionary<string, object> Data
    {
        get => _data;
        set => _data = value;
    }
}