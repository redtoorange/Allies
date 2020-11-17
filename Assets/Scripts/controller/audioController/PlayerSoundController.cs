using preferences;
using UnityEngine;

namespace controller.audioController
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSoundController : SoundController
    {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip gunShotSound;

        [SerializeField]
        private AudioClip hitSound;

        [SerializeField]
        private AudioClip deathSound;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayShootSound()
        {
            audioSource.PlayOneShot(gunShotSound, volume);
        }

        public void PlayHitSound()
        {
            audioSource.PlayOneShot(hitSound, volume);
        }

        public void PlayDeathSound()
        {
            audioSource.PlayOneShot(deathSound, volume);
        }
    }
}