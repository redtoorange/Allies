using System;
using character;
using controller;
using orders;
using scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace managers
{
    [Serializable]
    public enum GlobalZombieMode
    {
        DOCILE,
        AGGRESSIVE
    }

    public class ZombieManager : AIManager<ZombieController>
    {
        [SerializeField]
        private GameObject zombiePrefab;

        [SerializeField]
        private ZombieManagerConfig config;

        [SerializeField]
        private GlobalZombieMode currentMode = GlobalZombieMode.DOCILE;


        private void Start()
        {
            base.Start();

            foreach (ZombieController zomby in controllers)
            {
                zomby.OnNeedsOrders += CreateZombieOrders;
            }
        }

        /// <summary>
        /// Handle when the Game Round Phase changes.  This will usually be RECRUITMENT -> COMBAT which
        /// should trigger all zombies into Chase mode.
        /// </summary>
        /// <param name="phase"></param>
        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                foreach (ZombieController zomby in controllers)
                {
                    zomby.SetCurrentMode(ZombieMode.COMBAT);
                }
            }
        }


        private void CreateZombieOrders(ZombieController zombieController)
        {
            if (zombieController.NeedsOrder())
            {
                switch (zombieController.GetCurrentMode())
                {
                    case ZombieMode.SHAMBLE:
                        CreateShambleOrders(zombieController);
                        break;
                    case ZombieMode.CHASE:
                        CreateChaseOrders(zombieController);
                        break;
                    case ZombieMode.COMBAT:
                        CreateCombatOrders(zombieController);
                        break;
                }
            }
        }


        private void CreateShambleOrders(ZombieController zombieController)
        {
            Vector2 destination = new Vector2(
                Random.Range(-config.shambleRange, config.shambleRange),
                Random.Range(-config.shambleRange, config.shambleRange)
            );

            zombieController.AddOrder(new MoveOrder(zombieController.GetPosition() + destination,
                config.shambleSpeed));
            zombieController.AddOrder(new WaitOrder(Random.Range(config.shambleWait.x, config.shambleWait.y)));
        }

        private void CreateChaseOrders(ZombieController zombieController)
        {
            if (zombieController.GetTarget() != null)
            {
                zombieController.AddOrder(new WaitOrder(Random.Range(config.chaseWait.x, config.chaseWait.y)));
                zombieController.AddOrder(new ChaseOrder(zombieController.GetTarget(), config.chaseSpeed));
            }
        }

        private void CreateCombatOrders(ZombieController zombieController)
        {
            if (zombieController.GetTarget() != null)
            {
                zombieController.AddOrder(new ChaseOrder(zombieController.GetTarget(), config.combatSpeed));
            }
            else
            {
                Vector2 destination = new Vector2(
                    Random.Range(-config.shambleRange, config.shambleRange),
                    Random.Range(-config.shambleRange, config.shambleRange)
                );

                zombieController.AddOrder(new MoveOrder(zombieController.GetPosition() + destination,
                    config.combatSpeed));
            }
        }

        public void SpawnZombie(Vector2 position)
        {
            GameObject go = Instantiate(zombiePrefab, position, Quaternion.identity, transform);

            ZombieController controller = go.GetComponent<ZombieController>();
            AddController(controller);

            controller.OnNeedsOrders += CreateZombieOrders;
        }
    }
}