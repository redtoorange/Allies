using UnityEngine;

namespace preferences.settings
{
    public abstract class SettingSlider : MonoBehaviour
    {
        public abstract void Save();

        public abstract void Default();

        public abstract bool HasUnsavedChanges();
    }
}