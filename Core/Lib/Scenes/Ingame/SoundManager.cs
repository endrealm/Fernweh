using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Core.Scenes.Ingame
{
    public class SoundManager : ISoundPlayer
    {
        private Dictionary<string, SoundEffect> _sounds = new();
        private Dictionary<string, Song> _songs = new();
        private float _soundVolume = 1;
        private float _songVolume = 1;

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
            }

            // setup music settings
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = _songVolume;
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

            MediaPlayer.Play(_songs[name]);
        }
    }
}
