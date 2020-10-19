using character;
using controller;
using UnityEngine;

namespace managers
{
    public class ZombieManager : AIManager<ZombieController>
    {
        [SerializeField]
        private GameObject zombiePrefab;


        private ZombieState globalZombieState = ZombieState.Shamble;


        private void Start()
        {
            base.Start();
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                globalZombieState = ZombieState.Combat;
                for (int i = 0; i < controllers.Count; i++)
                {
                    controllers[i].SetState(ZombieState.Combat);
                }
            }
        }


        public void SpawnZombie(Vector2 position)
        {
            if (controllers.Count >= 50) return;

            GameObject go = Instantiate(zombiePrefab, position, Quaternion.identity, transform);
            ZombieController controller = go.GetComponent<ZombieController>();

            AddController(controller);
            gameRoundManager.SetCountDirty();
        }

        public void RemoveZombie(ZombieController zombieController)
        {
            RemoveController(zombieController);
            gameRoundManager.SetCountDirty();
        }

        public ZombieState GetGlobalState()
        {
            return globalZombieState;
        }
    }
}