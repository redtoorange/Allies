using preferences;
using UnityEngine;

namespace controller.audioController
{
    [RequireComponent(typeof(AudioSource))]
    public class InnocentSoundController : SoundController
    {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip convertedSound;

        [SerializeField]
        private AudioClip deathSound;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayDeathSound()
        {
            audioSource.PlayOneShot(deathSound, volume);
        }

        public void PlayConvertedSound()
        {
            audioSource.PlayOneShot(convertedSound, volume);
        }
    }
}