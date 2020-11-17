using System.Collections.Generic;
using preferences;
using UnityEngine;

namespace controller.audioController
{
    [RequireComponent(typeof(AudioSource))]
    public class ZombieSoundController : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip alertSound;

        [SerializeField]
        private AudioClip hitSound;

        [SerializeField]
        private AudioClip deathSound;

        [SerializeField]
        private List<AudioClip> audioClips;

        [SerializeField]
        private readonly float playCooldown = 2.5f;
        private float coolDown = 0.0f;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = PreferencesManager.Get(SettingsKeys.SOUND_VOLUME) / 100.0f;

            coolDown = Random.Range(playCooldown, playCooldown * 2);
        }

        public void PlayRandomSound()
        {
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
        }

        public void PlayHitSound()
        {
            audioSource.PlayOneShot(hitSound);
        }

        public void PlayAlertSound()
        {
            audioSource.PlayOneShot(alertSound);
        }

        public void PlayDeathSound()
        {
            audioSource.PlayOneShot(deathSound);
        }

        // private void Update()
        // {
        //     if (GameController.S.IsGamePaused())
        //     {
        //         return;
        //     }
        //
        //     if (audioSource.isPlaying)
        //     {
        //         return;
        //     }
        //     else if (coolDown > 0)
        //     {
        //         coolDown -= Time.deltaTime;
        //         return;
        //     }
        //
        //
        //     int roll = Random.Range(0, 100) + 1;
        //     if (roll > 90)
        //     {
        //         PlayRandomSound();
        //         coolDown = Random.Range(playCooldown, playCooldown * 2);
        //     }
        // }
    }
}