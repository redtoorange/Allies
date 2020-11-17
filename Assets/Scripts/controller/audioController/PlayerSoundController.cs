using preferences;
using UnityEngine;

namespace controller.audioController
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSoundController : MonoBehaviour
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
            audioSource.volume = PreferencesManager.Get(SettingsKeys.SOUND_VOLUME) / 100.0f;
        }

        public void PlayShootSound()
        {
            audioSource.PlayOneShot(gunShotSound);
        }

        public void PlayHitSound()
        {
            audioSource.PlayOneShot(hitSound);
        }

        public void PlayDeathSound()
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}