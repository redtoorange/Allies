using System;
using character;
using controller;
using orders;
using scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace managers
{

    public class ZombieManager : AIManager<ZombieController>
    {
        [SerializeField]
        private GameObject zombiePrefab;

        [SerializeField]
        private ZombieManagerConfig config;

        private ZombieState globalZombieState = ZombieState.Shamble;


        private void Start()
        {
            base.Start();

            foreach (ZombieController zombieController in controllers)
            {
                zombieController.SetSearchRange(config.shambleSearchRange);
                zombieController.OnNeedsOrders += CreateZombieOrders;
                zombieController.OnDeath += RemoveController;
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
                globalZombieState = ZombieState.Combat;
                foreach (ZombieController zombieController in controllers)
                {
                    zombieController.SetState(ZombieState.Combat);
                    zombieController.SetSearchRange(config.combatSearchRange);
                }
            }
        }


        private void CreateZombieOrders(ZombieController zombieController)
        {
            if (globalZombieState == ZombieState.Combat && zombieController.GetState() != ZombieState.Combat)
            {
                zombieController.SetState(ZombieState.Combat);
                zombieController.SetSearchRange(config.combatSearchRange);
            }

            if (zombieController.NeedsOrder())
            {
                switch (zombieController.GetState())
                {
                    case ZombieState.Shamble:
                        CreateShambleOrders(zombieController);
                        break;
                    case ZombieState.Chase:
                        CreateChaseOrders(zombieController);
                        break;
                    case ZombieState.Combat:
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
            GameCharacter target = zombieController.GetTarget();
            if (target != null)
            {
                zombieController.AddOrder(new WaitOrder(Random.Range(config.chaseWait.x, config.chaseWait.y)));
                zombieController.AddOrder(new ChaseOrder(target, config.chaseSpeed));
            }
        }

        private void CreateCombatOrders(ZombieController zombieController)
        {
            GameCharacter target = zombieController.GetTarget();
            if (target)
            {
                zombieController.AddOrder(new ChaseOrder(target, config.combatSpeed));
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
            if (controllers.Count >= 50) return;

            GameObject go = Instantiate(zombiePrefab, position, Quaternion.identity, transform);
            ZombieController controller = go.GetComponent<ZombieController>();

            AddController(controller);

            controller.OnNeedsOrders += CreateZombieOrders;
            controller.OnDeath += RemoveController;
        }

        public ZombieState GetGlobalState()
        {
            return globalZombieState;
        }
    }
}