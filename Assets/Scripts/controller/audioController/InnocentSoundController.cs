using preferences;
using UnityEngine;

namespace controller.audioController
{
    [RequireComponent(typeof(AudioSource))]
    public class InnocentSoundController : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip convertedSound;

        [SerializeField]
        private AudioClip deathSound;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = PreferencesManager.Get(SettingsKeys.SOUND_VOLUME) / 100.0f;
        }

        public void PlayDeathSound()
        {
            audioSource.PlayOneShot(deathSound);
        }

        public void PlayConvertedSound()
        {
            audioSource.PlayOneShot(convertedSound);
        }
    }
}