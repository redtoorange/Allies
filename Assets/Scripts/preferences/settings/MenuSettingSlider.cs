using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace preferences.settings
{
    public class MenuSettingSlider : SettingSlider
    {
        [SerializeField]
        private int defaultValue = 100;
        [SerializeField]
        private SettingsKeys settingKey = SettingsKeys.MUSIC_VOLUME;

        private int startingValue;
        private int currentValue;

        private Slider slider;
        private TMP_InputField inputField;

        private void Start()
        {
            currentValue = PreferencesManager.Get(settingKey);
            startingValue = currentValue;

            slider = GetComponentInChildren<Slider>();
            slider.SetValueWithoutNotify(currentValue);

            inputField = GetComponentInChildren<TMP_InputField>();
            inputField.SetTextWithoutNotify(currentValue + "");
        }

        public void OnSliderChanged()
        {
            currentValue = Mathf.RoundToInt(slider.value);
            inputField.SetTextWithoutNotify(currentValue + "");
        }

        public void OnInputChange()
        {
            currentValue = Mathf.RoundToInt(int.Parse(inputField.text.Trim()));
            slider.SetValueWithoutNotify(currentValue);
        }

        public override void Save()
        {
            PreferencesManager.Set(settingKey, currentValue);
        }

        public override void Default()
        {
            currentValue = defaultValue;
            slider.SetValueWithoutNotify(currentValue);
            inputField.SetTextWithoutNotify(currentValue + "");
        }

        public override bool HasUnsavedChanges()
        {
            return currentValue != startingValue;
        }
    }
}