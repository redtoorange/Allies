using UnityEngine;

namespace controller.audioController
{
    [RequireComponent(typeof(AudioSource))]
    public class ZombieSoundController : SoundController
    {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip alertSound;

        [SerializeField]
        private AudioClip hitSound;

        [SerializeField]
        private AudioClip deathSound;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayHitSound()
        {
            audioSource.PlayOneShot(hitSound, volume);
        }

        public void PlayAlertSound()
        {
            audioSource.PlayOneShot(alertSound, volume);
        }

        public void PlayDeathSound()
        {
            audioSource.PlayOneShot(deathSound, volume);
        }
    }
}