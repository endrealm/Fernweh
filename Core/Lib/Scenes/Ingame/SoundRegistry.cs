using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework.Audio;

namespace Core.Scenes.Ingame;

public class SoundRegistry : ISoundPlayer
{
    private readonly ContentRegistry _content;

    //private Dictionary<string, SoundEffect> _sounds = new();
    //private Dictionary<string, SoundEffect> _songs = new();
    private SoundEffectInstance _currentSong;

    public SoundRegistry(ContentRegistry content)
    {
        _content = content;
    }

    public void PlaySFX(string name, float pitch = 0)
    {
        if (!_content.wavs.ContainsKey(name)) return;

        var instance = _content.wavs[name].CreateInstance();
        instance.Volume = GameSettings.Instance.Sfx;
        instance.Pitch = pitch;
        instance.Play();
    }

    public void PlaySong(string name)
    {
        if (!_content.wavs.ContainsKey(name)) return;

        if (_currentSong != null) _currentSong.Dispose();

        _currentSong = _content.wavs[name].CreateInstance();
        _currentSong.Volume = GameSettings.Instance.Music;
        _currentSong.IsLooped = true;
        _currentSong.Play();
    }
}