using System;
using UnityEngine;

namespace Ky.SoundSystem
{
    [Serializable]
    public class Sound : MonoBehaviour
    {
        public AudioSource audioSource;
        public virtual float SoundLength => audioSource.clip.length * audioSource.pitch;

        public virtual void Play()
        {
            audioSource.Play();
        }

        public virtual void Play(float duration)
        {
            Play();
            SetPitch(duration / audioSource.clip.length);
        }

        public virtual void PlayAndDestroy()
        {
            Play();
            Destroy(gameObject, audioSource.clip.length + 0.1f);
        }

        public virtual void PlayAndDestroy(float duration)
        {
            Play();
            SetPitch(duration / audioSource.clip.length);
            Destroy(gameObject, duration + 0.1f);
        }

        public virtual void SetSoundVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public virtual void Stop()
        {
            audioSource.Stop();
        }

        public virtual void Pause()
        {
            audioSource.Pause();
        }

        public virtual void Resume()
        {
            audioSource.UnPause();
        }

        public virtual void SetPitch(float pitch)
        {
            audioSource.pitch = pitch;
        }
    }
}