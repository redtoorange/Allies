using controller;
using orders;
using scriptable;
using UnityEngine;

namespace managers.factories
{
    public abstract class InnocentOrderFactory
    {
        public static Order CreateWanderOrder(InnocentController controller, InnocentManagerConfig config)
        {
            var destination = new Vector2(
                Random.Range(-config.wanderRange, config.wanderRange),
                Random.Range(-config.wanderRange, config.wanderRange)
            );

            return new MoveOrder(controller.GetPosition() + destination, config.wanderSpeed);
        }

        public static Order CreateWaitOrder(InnocentController controller, InnocentManagerConfig config)
        {
            return new WaitOrder(Random.Range(config.wanderWait.x, config.wanderWait.y));
        }

        public static Order CreateRunOrders(InnocentController controller, InnocentManagerConfig config)
        {
            return new RunOrder(controller.GetThreat(), config.runSpeed);
        }

        public static Order CreateCombatOrders(InnocentController controller, InnocentManagerConfig config)
        {
            var destination = new Vector2(
                Random.Range(-config.combatRange, config.combatRange),
                Random.Range(-config.combatRange, config.combatRange)
            );

            return new MoveOrder(controller.GetPosition() + destination, config.combatSpeed);
        }
    }
}