using System;
using UnityEngine;

namespace preferences
{
    [Serializable]
    public enum SettingsKeys
    {
        SOUND_VOLUME,
        MUSIC_VOLUME
    }

    public class PreferencesManager
    {
        public static int Get(SettingsKeys key)
        {
            if (PlayerPrefs.HasKey(key.ToString()))
            {
                return PlayerPrefs.GetInt(key.ToString());
            }
            else
            {
                return 100;
            }
        }

        public static void Set(SettingsKeys key, int level)
        {
            int clamped = Mathf.Clamp(level, 0, 100);
            PlayerPrefs.SetInt(key.ToString(), clamped);
            Debug.Log("Wrote {'" + key.ToString() + "': '" + level + "'}");
        }
    }
}