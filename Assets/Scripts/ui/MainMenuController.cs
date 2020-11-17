using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private LeanTweenType easeType = LeanTweenType.easeInQuint;
        [SerializeField]
        private float transitionTime = 0.5f;

        private enum CurrentScreen
        {
            MAIN_MAIN,
            SETTINGS,
            LEVEL_SELECT
        }

        private CurrentScreen currentScreen = CurrentScreen.MAIN_MAIN;

        [SerializeField]
        private GameObject mainMenuPanel;
        [SerializeField]
        private GameObject settingsPanel;
        [SerializeField]
        private GameObject levelSelectPanel;

        private void Start()
        {
            Debug.Log(Application.persistentDataPath);
        }

        public void StartGameClicked()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitClicked()
        {
            Application.Quit();
        }

        public void TransitionToSettings()
        {
            currentScreen = CurrentScreen.SETTINGS;
            TransitionScreens(mainMenuPanel, settingsPanel, -1500);
        }

        public void TransitionToLevelSelect()
        {
            currentScreen = CurrentScreen.LEVEL_SELECT;
            TransitionScreens(mainMenuPanel, levelSelectPanel, 1500);
        }

        public void TransitionToMainMenu()
        {
            if (currentScreen == CurrentScreen.SETTINGS)
            {
                TransitionScreens(mainMenuPanel, settingsPanel, 1500);
            }
            else if (currentScreen == CurrentScreen.LEVEL_SELECT)
            {
                TransitionScreens(mainMenuPanel, levelSelectPanel, -1500);
            }
            
            currentScreen = CurrentScreen.MAIN_MAIN;
        }

        private void TransitionScreens(GameObject screenA, GameObject screenB, float delta)
        {
            Vector2 screenAPos = screenA.transform.position;
            screenAPos.x += delta;

            Vector2 screenBPos = screenB.transform.position;
            screenBPos.x += delta;

            LeanTween.move(screenA, screenAPos, transitionTime).setEase(easeType);
            LeanTween.move(screenB, screenBPos, transitionTime).setEase(easeType);
        }
    }
}