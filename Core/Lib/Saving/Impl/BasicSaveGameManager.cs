using System.IO;

namespace Core.Saving.Impl;

public class BasicSaveGameManager: ISaveGameManager
{
    private readonly string _path;

    public BasicSaveGameManager(string path)
    {
        _path = path;
        Directory.CreateDirectory(_path);
    }
    
    public bool Exists(string name)
    {
        return File.Exists(Path.Combine(_path, name));
    }

    public IGameSave CreateNew(string name)
    {
        var game = new BasicGameSave(Path.Combine(_path, name));
        game.Save();
        return game;
    }

    public IGameSave Load(string name)
    {
        var game = new BasicGameSave(Path.Combine(_path, name));
        game.Load();
        return game;
    }
}