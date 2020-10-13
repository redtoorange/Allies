using System;
using UnityEngine;

namespace managers
{
    public class PlayerManager : MonoBehaviour
    {
        private GameRoundManager gameRoundManager;
        private bool combatStarted = false;
        
        private void Start()
        {
            gameRoundManager = GetComponentInParent<GameManager>().GetGameRoundManager();
        }

        public void CheckToStartCombat()
        {
            if (!combatStarted)
            {
                gameRoundManager.TriggerCombat();
            }    
        }
    }
}