namespace Core.Saving;

public interface ISaveGameManager
{
    bool Exists(string name);
    IGameSave CreateNew(string name);
    IGameSave Load(string name);
}