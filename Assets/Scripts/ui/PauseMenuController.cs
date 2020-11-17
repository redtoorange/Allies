using controller;
using preferences.unlock;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField]
        private LeanTweenType easeType = LeanTweenType.easeOutBack;
        [SerializeField]
        private float transitionTime = 0.5f;

        [SerializeField]
        private float fadeTime = 0.25f;
        [SerializeField]
        private LeanTweenType fadeEase = LeanTweenType.easeOutQuart;

        private LevelUnlocker levelUnlocker;
        private CanvasGroup canvasGroup;

        private enum CurrentScreen
        {
            MAIN_MAIN,
            SETTINGS
        }

        private enum CurrentState
        {
            OPEN,
            CLOSED
        }

        private CurrentScreen currentScreen = CurrentScreen.MAIN_MAIN;
        private CurrentState currentState = CurrentState.CLOSED;

        [SerializeField]
        private GameObject pauseMenuPanel;
        [SerializeField]
        private GameObject settingsPanel;
        [SerializeField]
        private GameObject background;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            levelUnlocker = FindObjectOfType<LevelUnlocker>();

            CloseNow();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentState == CurrentState.CLOSED)
                {
                    FadeOpen();
                }
                else
                {
                    FadeClosed();
                }
            }
        }

        public void CloseNow()
        {
            if (LeanTween.isTweening(gameObject))
            {
                LeanTween.cancel(gameObject);
            }

            currentState = CurrentState.CLOSED;

            LeanTween.alphaCanvas(canvasGroup, 0.0f, 0.0f)
                .setEase(fadeEase)
                .setOnComplete(() =>
                {
                    pauseMenuPanel.SetActive(false);
                    settingsPanel.SetActive(false);
                    background.SetActive(false);

                    GameController.S.SetGamePaused(false);

                    if (currentScreen != CurrentScreen.MAIN_MAIN)
                    {
                        MoveScreens(pauseMenuPanel, settingsPanel, 1500);
                        currentScreen = CurrentScreen.MAIN_MAIN;
                    }
                });
        }

        public void FadeClosed()
        {
            if (LeanTween.isTweening(gameObject))
            {
                return;
            }

            currentState = CurrentState.CLOSED;
            LeanTween.alphaCanvas(canvasGroup, 0.0f, fadeTime)
                .setEase(fadeEase)
                .setOnComplete(() =>
                {
                    pauseMenuPanel.SetActive(false);
                    settingsPanel.SetActive(false);
                    background.SetActive(false);

                    GameController.S.SetGamePaused(false);

                    if (currentScreen != CurrentScreen.MAIN_MAIN)
                    {
                        MoveScreens(pauseMenuPanel, settingsPanel, 1500);
                        currentScreen = CurrentScreen.MAIN_MAIN;
                    }
                });
        }

        public void FadeOpen()
        {
            if (LeanTween.isTweening(gameObject))
            {
                return;
            }

            GameController.S.SetGamePaused(true);
            currentState = CurrentState.OPEN;

            pauseMenuPanel.SetActive(true);
            settingsPanel.SetActive(true);
            background.SetActive(true);
            LeanTween.alphaCanvas(canvasGroup, 1, fadeTime).setEase(fadeEase);
        }

        public void OnResumeClicked()
        {
            FadeClosed();
        }

        public void OnRestartClicked()
        {
            SceneManager.LoadScene(levelUnlocker.GetLevelId());
        }

        public void OnMainMenuClicked()
        {
            SceneManager.LoadScene(0);
        }

        public void OnSettingsClicked()
        {
            TransitionToSettings();
        }

        public void TransitionToSettings()
        {
            currentScreen = CurrentScreen.SETTINGS;
            TransitionScreens(pauseMenuPanel, settingsPanel, -1500);
        }

        public void TransitionToPauseMenu()
        {
            TransitionScreens(pauseMenuPanel, settingsPanel, 1500);
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

        private void MoveScreens(GameObject screenA, GameObject screenB, float delta)
        {
            Vector3 screenAPos = screenA.transform.position;
            screenAPos.x += delta;

            Vector3 screenBPos = screenB.transform.position;
            screenBPos.x += delta;

            screenA.transform.position = screenAPos;
            screenB.transform.position = screenBPos;
        }
    }
}