using bullet;
using UnityEngine;

namespace managers
{
    public class GameManager : MonoBehaviour
    {
        private InnocentManager innocentManager = null;
        private AllyManager allyManager = null;
        private ZombieManager zombieManager = null;
        private PlayerManager playerManager = null;
        private BulletManager bulletManager = null;

        private void Start()
        {
            innocentManager = GetComponentInChildren<InnocentManager>();
            allyManager = GetComponentInChildren<AllyManager>();
            zombieManager = GetComponentInChildren<ZombieManager>();
            playerManager = GetComponentInChildren<PlayerManager>();
            bulletManager = GetComponentInChildren<BulletManager>();
        }

        public InnocentManager GetInnocentManager() => innocentManager;
        public AllyManager GetAllyManager() => allyManager;
        public ZombieManager GetZombieManager() => zombieManager;
        public PlayerManager GetPlayerManager() => playerManager;
        public BulletManager GetBulletManager() => bulletManager;
    }
}