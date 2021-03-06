﻿using managers;
using preferences.unlock;
using ui.manager;
using UnityEngine;

namespace controller
{
    public class GameController : MonoBehaviour
    {
        public static GameController S;

        private UIManager uiManager;
        private SystemManager systemManager;
        private LevelDataContainer levelDataContainer;

        [SerializeField]
        private bool startGamePaused = true;
        private bool gamePaused = true;

        private void Awake()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                Debug.LogError("Cannot have two GameControllers");
                Destroy(gameObject);
                gameObject.SetActive(false);
            }

            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("Missing UIManager in GameController");
            }

            systemManager = FindObjectOfType<SystemManager>();
            if (systemManager == null)
            {
                Debug.LogError("Missing SystemManager in GameController");
            }

            levelDataContainer = FindObjectOfType<LevelDataContainer>();
            if (levelDataContainer == null)
            {
                Debug.LogError("Missing LevelUnlocker in GameController");
            }
        }

        private void Start()
        {
            if (startGamePaused)
            {
                gamePaused = true;
            }
            else
            {
                gamePaused = false;
            }
        }

        public bool IsGamePaused()
        {
            return gamePaused;
        }

        public void SetGamePaused(bool paused)
        {
            gamePaused = paused;
        }
    }
}