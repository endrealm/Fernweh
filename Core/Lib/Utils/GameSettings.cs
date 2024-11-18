namespace Core.Utils;

public class GameSettings
{
    public delegate void SettingsChange();

    public bool Fullscreen = false;

    public float Music = 0f;
    public float Sfx = 0f;

    public bool showFPS = true;

    public float TypingSpeed = 0.01f;

    public GameSettings()
    {
        Instance = this;
    }

    public static GameSettings Instance { get; private set; }
    public event SettingsChange OnVideoSettingsChanged;

    public void UpdateVideoSettings()
    {
        OnVideoSettingsChanged.Invoke();
    }
}