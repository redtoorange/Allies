using UnityEngine;

namespace managers
{
    public class PlayerManager : MonoBehaviour
    {
        private readonly bool combatStarted = false;
        private GameRoundManager gameRoundManager;

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