using UnityEngine;

namespace preferences
{
    public class PreferencesManager
    {
        private static readonly string VOLUME_KEY = "VOLUME";

        public static float GetVolume()
        {
            if (PlayerPrefs.HasKey(VOLUME_KEY))
            {
                return PlayerPrefs.GetFloat(VOLUME_KEY);
            }
            else
            {
                return 1.0f;
            }
        }

        public static void SetVolume(float level)
        {
            float clamped = Mathf.Clamp(level, 0.0f, 1.0f);
            PlayerPrefs.SetFloat(VOLUME_KEY, clamped);
        }
    }
}