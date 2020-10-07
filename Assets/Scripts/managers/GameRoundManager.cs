using System;
using ui;
using UnityEngine;
using UnityEngine.Events;

namespace managers
{
    [Serializable]
    public enum GameRoundPhase
    {
        Recruitment,
        Combat,
        Done
    }

    public class GameRoundManager : MonoBehaviour
    {
        [SerializeField]
        private CountDownTimer recruitmentCountdown;

        [SerializeField]
        private float recruitmentRoundLength = 30.0f;

        [SerializeField]
        private GameRoundPhase currentPhase = GameRoundPhase.Recruitment;

        public UnityEvent<GameRoundPhase> OnPhaseChange;
        private void Start()
        {
            if (recruitmentCountdown)
            {
                recruitmentCountdown.OnTimeOut.AddListener(RecruitmentEnded);
                recruitmentCountdown.SetTime(recruitmentRoundLength);
                recruitmentCountdown.gameObject.SetActive(true);
                recruitmentCountdown.StartTimer();
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

            currentPhase = GameRoundPhase.Combat;
            OnPhaseChange.Invoke(currentPhase);
        }
    }
}