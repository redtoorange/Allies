using System;
using UnityEngine;

namespace managers
{
    public class PlayerManager : MonoBehaviour
    {
        public Action playerDied;
        
        private bool combatStarted = false;
        private GameRoundManager gameRoundManager;

        private void Start()
        {
            gameRoundManager = GetComponentInParent<SystemManager>().GetGameRoundManager();
            gameRoundManager.OnPhaseChange += HandlePhaseChange;
        }

        private void HandlePhaseChange(GameRoundPhase newPhase)
        {
            if (newPhase == GameRoundPhase.Combat)
            {
                combatStarted = true;
            }
        }

        public void CheckToStartCombat()
        {
            if (!combatStarted)
            {
                combatStarted = true;
                gameRoundManager.TriggerCombat();
            }
        }

        public void PlayerDied()
        {
            playerDied.Invoke();
        }
    }
}