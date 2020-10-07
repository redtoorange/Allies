using UnityEngine;

namespace managers
{
    public class GameManager : MonoBehaviour
    {
        private InnocentManager innocentManager = null;
        private AllyManager allyManager = null;
        private ZombieManager zombieManager = null;

        private void Start()
        {
            innocentManager = GetComponentInChildren<InnocentManager>();
            allyManager = GetComponentInChildren<AllyManager>();
            zombieManager = GetComponentInChildren<ZombieManager>();
        }

        public InnocentManager InnocentManager => innocentManager;
        public AllyManager AllyManager => allyManager;
        public ZombieManager ZombieManager => zombieManager;
    }
}