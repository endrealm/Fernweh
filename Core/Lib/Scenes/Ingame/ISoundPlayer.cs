using Core.Content;

namespace Core.Scenes.Ingame
{
    public interface ISoundPlayer
    {
        void ScanForAudio(ContentLoader content);
        void PlaySFX(string name, float pitch = 0);
        void PlaySong(string name);
    }
}