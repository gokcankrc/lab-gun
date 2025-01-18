using UnityEngine.Audio;

namespace Ky.SoundSystem.Game
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioMixer audioMixer;

        private const string MASTER_VOLUME_KEY = "masterVolume";
        private const string MUSIC_VOLUME_KEY = "musicVolume";
        private const string SOUND_VOLUME_KEY = "soundVolume";

        public float masterVolume;
        public float musicVolume;
        public float soundVolume;
    }
}