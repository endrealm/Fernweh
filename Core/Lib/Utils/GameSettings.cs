namespace Core.Utils;

public class GameSettings
{
    public static GameSettings Instance { get; private set; }

    public delegate void SettingsChange();
    public event SettingsChange OnVideoSettingsChanged;

    public float Music = 0f;
    public float Sfx = 0f;

    public bool Fullscreen = false;

    public float TypingSpeed = 0.01f;

    public GameSettings()
    {
        Instance = this;
    }

    public void UpdateVideoSettings()
    {
        OnVideoSettingsChanged.Invoke();
    }
}