using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
        _data = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
    }

    public Dictionary<string, object> Data
    {
        get => _data;
        set => _data = value;
    }
}