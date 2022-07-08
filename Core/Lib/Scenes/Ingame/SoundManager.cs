using Core.Content;
using Core.Scenes.Modding;
using Core.Utils;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Core.Scenes.Ingame
{
    public class SoundManager : ISoundPlayer
    {
        private Dictionary<string, SoundEffect> _sounds = new();
        private Dictionary<string, SoundEffect> _songs = new();
        private SoundEffectInstance _currentSong;

        public void ScanForAudio(ContentLoader content)
        {
            // load each sound file and sort from sfx to songs
            List<Mod> mods = content.ModLoader.ActiveModOrder;
            foreach (Mod mod in mods)
            {
                string[] soundFiles = mod.Archive.LoadAllFiles("*.wav");
                foreach (var file in soundFiles)
                {
                    var name = System.IO.Path.GetFileName(file).Replace(".wav", "");
                    var sound = SoundEffect.FromStream(mod.Archive.LoadFileAsStream(file));

                    if (file.Contains("sfx"))
                        _sounds.Add(name, sound);
                    else if (file.Contains("music"))
                        _songs.Add(name, sound);
                }
            }
        }

        public void PlaySFX(string name, float pitch = 0)
        {
            if (!_sounds.ContainsKey(name)) return;

            var instance = _sounds[name].CreateInstance();
            instance.Volume = GameSettings.Instance.Sfx;
            instance.Pitch = pitch;
            instance.Play();
        }

        public void PlaySong(string name)
        {
            if (!_songs.ContainsKey(name)) return;

            if(_currentSong != null) _currentSong.Dispose();

            _currentSong = _songs[name].CreateInstance();
            _currentSong.Volume = GameSettings.Instance.Music;
            _currentSong.IsLooped = true;
            _currentSong.Play();
        }
    }
}
