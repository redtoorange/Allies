using preferences;
using preferences.settings;
using UnityEngine;

namespace controller.audioController
{
    public class SoundController : MonoBehaviour
    {
        protected float volume = 1.0f;
        protected SettingsMenuController settingsMenuController;

        protected void Awake()
        {
            settingsMenuController = FindObjectOfType<SettingsMenuController>();
            if (!settingsMenuController)
            {
                Debug.LogError("SoundController cannot find a SettingsMenuController for volume events");
            }

            volume = PreferencesManager.Get(SettingsKeys.SOUND_VOLUME) / 100.0f;
        }

        private void OnEnable()
        {
            settingsMenuController.SettingsChanged += SettingsChanged;
        }

        private void SettingsChanged()
        {
            volume = PreferencesManager.Get(SettingsKeys.SOUND_VOLUME) / 100.0f;
        }

        private void OnDisable()
        {
            settingsMenuController.SettingsChanged -= SettingsChanged;
        }
    }
}