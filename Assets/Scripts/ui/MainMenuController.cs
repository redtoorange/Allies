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

        private float leftGutterPositionX;
        private float centerPositionX;
        private float rightGutterPositionX;

        private void Start()
        {
            leftGutterPositionX = levelSelectPanel.transform.position.x;
            centerPositionX = mainMenuPanel.transform.position.x;
            rightGutterPositionX = settingsPanel.transform.position.x;
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
            TransitionScreensLeft(mainMenuPanel, settingsPanel);
        }

        public void TransitionToLevelSelect()
        {
            currentScreen = CurrentScreen.LEVEL_SELECT;
            TransitionScreensRight(mainMenuPanel, levelSelectPanel);
        }

        public void TransitionToMainMenu()
        {
            if (currentScreen == CurrentScreen.SETTINGS)
            {
                TransitionScreensRight(settingsPanel, mainMenuPanel);

            }
            else if (currentScreen == CurrentScreen.LEVEL_SELECT)
            {
                TransitionScreensLeft(levelSelectPanel, mainMenuPanel);

            }

            currentScreen = CurrentScreen.MAIN_MAIN;
        }

        private void TransitionScreensLeft(GameObject screenA, GameObject screenB)
        {
            Vector2 screenAPos = screenA.transform.position;
            screenAPos.x = leftGutterPositionX;

            Vector2 screenBPos = screenB.transform.position;
            screenBPos.x = centerPositionX;

            LeanTween.move(screenA, screenAPos, transitionTime).setEase(easeType);
            LeanTween.move(screenB, screenBPos, transitionTime).setEase(easeType);
        }
        
        private void TransitionScreensRight(GameObject screenA, GameObject screenB)
        {
            Vector2 screenAPos = screenA.transform.position;
            screenAPos.x = rightGutterPositionX;

            Vector2 screenBPos = screenB.transform.position;
            screenBPos.x = centerPositionX;

            LeanTween.move(screenA, screenAPos, transitionTime).setEase(easeType);
            LeanTween.move(screenB, screenBPos, transitionTime).setEase(easeType);
        }
    }
}