using System.Collections.Generic;

namespace Core.Saving;

public interface IGameSave
{
    void Save();
    void Load();
    
    public Dictionary<string, object> Data { get; set; }
}