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
                foreach (var zombieController in controllers) zombieController.SetState(ZombieState.Combat);
            }
        }


        public void SpawnZombie(Vector2 position)
        {
            if (controllers.Count >= 50) return;

            var go = Instantiate(zombiePrefab, position, Quaternion.identity, transform);
            var controller = go.GetComponent<ZombieController>();

            AddController(controller);
        }

        public void RemoveZombie(ZombieController zombieController)
        {
            RemoveController(zombieController);
        }

        public ZombieState GetGlobalState()
        {
            return globalZombieState;
        }
    }
}