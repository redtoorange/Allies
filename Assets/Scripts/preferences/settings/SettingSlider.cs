using UnityEngine;

namespace preferences.settings
{
    public abstract class SettingSlider : MonoBehaviour
    {
        public abstract void Save();

        public abstract void Default();

        public abstract void Reset();

        public abstract bool HasUnsavedChanges();
    }
}