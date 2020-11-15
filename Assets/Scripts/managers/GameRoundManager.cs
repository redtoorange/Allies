using System;
using controller;
using ui;
using UnityEngine;

namespace managers
{
    [Serializable]
    public enum GameRoundPhase
    {
        Recruitment,
        Combat,
        Won,
        Lost
    }

    public class GameRoundManager : MonoBehaviour
    {
        [SerializeField]
        private bool loseOnNoSurvivors = true;
        [SerializeField]
        private bool winOnNoZombies = true;

        private CountDownTimer recruitmentCountdown = null;
        private InnocentCounter innocentCounter = null;
        
        [SerializeField]
        private float recruitmentRoundLength = 30.0f;
        [SerializeField]
        private GameRoundPhase currentPhase = GameRoundPhase.Recruitment;

        private SystemManager systemManager = null;
        private InnocentManager innocentManager = null;
        private AllyManager allyManager = null;
        private ZombieManager zombieManager = null;

        private bool countDirty = true;
        private int survivorCount = 0;
        private int zombieCount = 0;

        public event Action<GameRoundPhase> OnPhaseChange;

        private void Start()
        {
            systemManager = GetComponentInParent<SystemManager>();
            
            innocentManager = systemManager.GetInnocentManager();
            allyManager = systemManager.GetAllyManager();
            zombieManager = systemManager.GetZombieManager();
            
            recruitmentCountdown = systemManager.GetUIManager().GetRecruitmentTimer();
            if (recruitmentCountdown)
            {
                recruitmentCountdown.OnTimeOut.AddListener(RecruitmentEnded);
                recruitmentCountdown.SetTime(recruitmentRoundLength);
                recruitmentCountdown.gameObject.SetActive(true);
                recruitmentCountdown.StartTimer();
            }
            
            innocentCounter = systemManager.GetUIManager().GetInnocentCounter();
            if (innocentCounter)
            {
                innocentCounter.SetCounter(survivorCount);
            }
        }

        private void OnDisable()
        {
            if (recruitmentCountdown)
            {
                recruitmentCountdown.OnTimeOut.RemoveListener(RecruitmentEnded);
            }
        }


        private void RecruitmentEnded()
        {
            if (recruitmentCountdown)
            {
                recruitmentCountdown.gameObject.SetActive(false);
            }

            SetPhase(GameRoundPhase.Combat);
        }

        public void TriggerCombat()
        {
            RecruitmentEnded();
        }

        public void SetCountDirty()
        {
            countDirty = true;
        }

        public void SetPhase(GameRoundPhase newPhase)
        {
            if (newPhase != currentPhase && (currentPhase != GameRoundPhase.Lost || currentPhase != GameRoundPhase.Won))
            {
                Debug.Log("Changing from " + currentPhase + " to " + newPhase);
                currentPhase = newPhase;
                OnPhaseChange?.Invoke(currentPhase);
            }
        }

        private void LateUpdate()
        {
            if (GameController.S.IsGamePaused()) return;
            
            if (currentPhase == GameRoundPhase.Won || currentPhase == GameRoundPhase.Lost)
                return;

            if (countDirty)
            {
                countDirty = false;

                zombieCount = zombieManager.GetControllerCount();

                survivorCount = innocentManager.GetControllerCount() + allyManager.GetControllerCount();
                innocentCounter.SetCounter(survivorCount);
            }

            if (zombieCount == 0 && winOnNoZombies)
            {
                GameController.S.SetGamePaused(true);
                systemManager.GetUIManager().GetModalUIController().DisplayWinScreen();
                SetPhase(GameRoundPhase.Won);
            }
            else if (survivorCount == 0 && loseOnNoSurvivors)
            {
                GameController.S.SetGamePaused(true);
                systemManager.GetUIManager().GetModalUIController().DisplayLoseScreen();
                SetPhase(GameRoundPhase.Lost);
            }
        }
    }
}