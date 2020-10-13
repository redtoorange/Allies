using controller;
using orders;
using scriptable;
using UnityEngine;

namespace managers.factories
{
    public abstract class ZombieOrderFactory
    {
        public static Order CreateShambleOrder(ZombieController controller, ZombieManagerConfig config)
        {
            var destination = new Vector2(
                Random.Range(-config.shambleRange, config.shambleRange),
                Random.Range(-config.shambleRange, config.shambleRange)
            );

            return new MoveOrder(controller.GetPosition() + destination, config.shambleSpeed);
        }

        public static Order CreateWaitOrder(ZombieController controller, ZombieManagerConfig config)
        {
            return new WaitOrder(Random.Range(config.shambleWait.x, config.shambleWait.y));
        }

        public static Order CreateChaseOrders(ZombieController controller, ZombieManagerConfig config)
        {
            return new ChaseOrder(controller.GetTarget(), config.chaseSpeed);
        }

        public static Order CreateCombatOrders(ZombieController controller, ZombieManagerConfig config)
        {
            var target = controller.GetTarget();
            if (target)
            {
                return new ChaseOrder(target, config.combatSpeed);
            }

            var destination = new Vector2(
                Random.Range(-config.shambleRange, config.shambleRange),
                Random.Range(-config.shambleRange, config.shambleRange)
            );

            return new MoveOrder(controller.GetPosition() + destination, config.combatSpeed);
        }
    }
}