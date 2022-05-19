namespace Core.Utils;

public class GameSettings
{
    public static GameSettings Instance { get; private set; }
    public float Volume = 0f;

    public GameSettings()
    {
        Instance = this;
    }

    public float GetMusicVolume()
    {
        return Volume;
    }

}