using bullet;
using ui;
using UnityEngine;

namespace managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager S;
        
        [SerializeField]
        private bool gamePaused = true;

        private AllyManager allyManager;
        private BulletManager bulletManager;
        private GameRoundManager gameRoundManager;
        private InnocentManager innocentManager;
        private PlayerManager playerManager;
        private ZombieManager zombieManager;
        
        private UIController uiController;

        private void Awake()
        {
            if (S == null)
            {
                S = this;
            }
            
            gameRoundManager = GetComponentInChildren<GameRoundManager>();
            innocentManager = GetComponentInChildren<InnocentManager>();
            allyManager = GetComponentInChildren<AllyManager>();
            zombieManager = GetComponentInChildren<ZombieManager>();
            playerManager = GetComponentInChildren<PlayerManager>();
            bulletManager = GetComponentInChildren<BulletManager>();

            uiController = FindObjectOfType<UIController>();
        }

        public GameRoundManager GetGameRoundManager()
        {
            return gameRoundManager;
        }

        public InnocentManager GetInnocentManager()
        {
            return innocentManager;
        }

        public AllyManager GetAllyManager()
        {
            return allyManager;
        }

        public ZombieManager GetZombieManager()
        {
            return zombieManager;
        }

        public PlayerManager GetPlayerManager()
        {
            return playerManager;
        }

        public BulletManager GetBulletManager()
        {
            return bulletManager;
        }

        public bool IsGamePaused()
        {
            return gamePaused;
        }

        public void SetGamePaused(bool paused)
        {
            gamePaused = paused;
        }

        public UIController GetUIController()
        {
            return uiController;
        }
    }
}