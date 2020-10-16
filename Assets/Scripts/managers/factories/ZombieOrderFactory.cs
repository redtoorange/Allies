using character;
using controller;
using orders;
using scriptable;
using UnityEngine;

namespace managers.factories
{
    public abstract class ZombieOrderFactory
    {
        public static Order CreateShambleOrder(ZombieController controller, ZombieConfig config)
        {
            Vector2 destination = new Vector2(
                Random.Range(-config.shambleRange, config.shambleRange),
                Random.Range(-config.shambleRange, config.shambleRange)
            );

            return new MoveOrder(controller.GetPosition() + destination, config.shambleSpeed);
        }

        public static Order CreateWaitOrder(ZombieController controller, ZombieConfig config)
        {
            return new WaitOrder(Random.Range(config.shambleWait.x, config.shambleWait.y));
        }

        public static Order CreateChaseOrders(ZombieController controller, ZombieConfig config)
        {
            GameCharacter target = controller.GetClosestTarget();
            if (target)
            {
                return new ChaseOrder(controller.GetClosestTarget(), config.chaseSpeed);
            }

            return new WaitOrder(0);
        }

        public static Order CreateCombatOrders(ZombieController controller, ZombieConfig config)
        {
            GameCharacter target = controller.GetClosestTarget();
            if (target)
            {
                return new ChaseOrder(target, config.combatSpeed);
            }

            Vector2 destination = new Vector2(
                Random.Range(-config.shambleRange, config.shambleRange),
                Random.Range(-config.shambleRange, config.shambleRange)
            );

            return new MoveOrder(controller.GetPosition() + destination, config.combatSpeed);
        }
    }
}