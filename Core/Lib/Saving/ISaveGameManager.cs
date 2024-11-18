using System.Collections.Generic;

namespace Core.Saving;

public interface ISaveGameManager
{
    bool Exists(string name);
    IGameSave CreateNew(string name);
    IGameSave Load(string name);

    /// <summary>
    ///     Games saves are not loaded and will have to be loaded
    /// </summary>
    /// <returns></returns>
    List<IGameSave> ListAll();
}