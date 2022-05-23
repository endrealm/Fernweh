namespace Core.Utils;

public class GameSettings
{
    public static GameSettings Instance { get; private set; }

    public delegate void SettingsChange();
    public event SettingsChange OnVideoSettingsChanged;

    private bool _fullscreen = false;
    public bool Fullscreen
    {
        get { return _fullscreen; }
        set { OnVideoSettingsChanged.Invoke(); _fullscreen = value; }
    }

    public float Music = 0f;
    public float Sfx = 0f;

    public GameSettings()
    {
        Instance = this;
    }
}