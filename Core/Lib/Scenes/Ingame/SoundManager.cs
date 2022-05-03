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
        private float _soundVolume = 0;
        private float _songVolume = 0;

        public void ScanForAudio(ContentLoader content)
        {
            // load each sound file and sort from sfx to songs
            List<IArchiveLoader> mods = content.GetMods();
            foreach (IArchiveLoader mod in mods)
            {
                string[] sfxFiles = mod.LoadAllFiles("*.sfx.wav");
                string[] sngFiles = mod.LoadAllFiles("*.sng.wav");
                foreach (var file in sfxFiles)
                    _sounds.Add(System.IO.Path.GetFileName(file).Replace(".sfx.wav", ""), SoundEffect.FromStream(mod.LoadFileAsStream(file)));

                foreach (var file in sngFiles)
                    _songs.Add(System.IO.Path.GetFileName(file).Replace(".sng.wav", ""), SoundEffect.FromStream(mod.LoadFileAsStream(file)));
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
