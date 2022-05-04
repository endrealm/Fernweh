using Core.Content;
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
        private float _soundVolume = 0f;
        private float _songVolume = 0f;

        public void ScanForAudio(ContentLoader content)
        {
            // load each sound file and sort from sfx to songs
            List<IArchiveLoader> mods = content.GetMods();
            foreach (IArchiveLoader mod in mods)
            {
                string[] soundFiles = mod.LoadAllFiles("*.wav");
                foreach (var file in soundFiles)
                {
                    var name = System.IO.Path.GetFileName(file).Replace(".wav", "");
                    var sound = SoundEffect.FromStream(mod.LoadFileAsStream(file));

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
            instance.Volume = _soundVolume;
            instance.Pitch = pitch;
            instance.Play();
        }

        public void PlaySong(string name)
        {
            if (!_songs.ContainsKey(name)) return;

            var instance = _songs[name].CreateInstance();
            instance.Volume = _songVolume;
            instance.IsLooped = true;
            instance.Play();
        }
    }
}
