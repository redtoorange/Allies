using bullet;
using ui;
using UnityEngine;

namespace managers
{
    public class SystemManager : MonoBehaviour
    {
       

        private AllyManager allyManager;
        private BulletManager bulletManager;
        private GameRoundManager gameRoundManager;
        private InnocentManager innocentManager;
        private PlayerManager playerManager;
        private ZombieManager zombieManager;

        private UIManager uiManager;

        private void Awake()
        {
            gameRoundManager = GetComponentInChildren<GameRoundManager>();
            innocentManager = GetComponentInChildren<InnocentManager>();
            allyManager = GetComponentInChildren<AllyManager>();
            zombieManager = GetComponentInChildren<ZombieManager>();
            playerManager = GetComponentInChildren<PlayerManager>();
            bulletManager = GetComponentInChildren<BulletManager>();

            uiManager = FindObjectOfType<UIManager>();
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

       

        public UIController GetUIController()
        {
            return uiManager.GetUIController();
        }
    }
}