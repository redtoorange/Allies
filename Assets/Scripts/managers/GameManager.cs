using bullet;
using UnityEngine;

namespace managers
{
    public class GameManager : MonoBehaviour
    {
        private AllyManager allyManager;
        private BulletManager bulletManager;
        private InnocentManager innocentManager;
        private PlayerManager playerManager;
        private ZombieManager zombieManager;

        private void Start()
        {
            innocentManager = GetComponentInChildren<InnocentManager>();
            allyManager = GetComponentInChildren<AllyManager>();
            zombieManager = GetComponentInChildren<ZombieManager>();
            playerManager = GetComponentInChildren<PlayerManager>();
            bulletManager = GetComponentInChildren<BulletManager>();
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
    }
}