using System.Collections.Generic;

namespace Core.Saving;

public interface IGameSave
{
    public Dictionary<string, object> Data { get; set; }
    string Name { get; }
    void Save();
    void Load();
}