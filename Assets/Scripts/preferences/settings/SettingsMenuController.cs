using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace preferences.settings
{
    public class SettingsMenuController : MonoBehaviour
    {
        [SerializeField]
        private Button saveButton;
        private List<MenuSettingSlider> sliders;

        private void Start()
        {
            sliders = new List<MenuSettingSlider>(GetComponentsInChildren<MenuSettingSlider>());
        }

        private void Update()
        {
            CheckSliders();
        }

        private void CheckSliders()
        {
            bool modified = false;
            for (int i = 0; i < sliders.Count && !modified; i++)
            {
                modified = sliders[i].HasUnsavedChanges();
            }

            saveButton.interactable = modified;
        }

        public void OnSaveClicked()
        {
            for (int i = 0; i < sliders.Count; i++)
            {
                sliders[i].Save();
            }
        }

        public void OnDefaultClicked()
        {
            for (int i = 0; i < sliders.Count; i++)
            {
                sliders[i].Default();
            }
        }

        public void OnCancelClicked()
        {
            for (int i = 0; i < sliders.Count; i++)
            {
                sliders[i].Reset();
            }
        }
    }
}