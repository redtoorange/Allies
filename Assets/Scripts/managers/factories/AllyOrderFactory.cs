using character;
using controller;
using orders;
using scriptable;
using UnityEngine;

namespace managers.factories
{
    public abstract class AllyOrderFactory
    {
        // What are neutral orders?  Should this be a small wander? Stay away from zombies?
        public static Order CreateNeutralOrders(AllyController allyController, AllyManagerConfig config)
        {
            // Vector2 destination = new Vector2(
            //     Random.Range(-config.shambleRange, config.shambleRange),
            //     Random.Range(-config.shambleRange, config.shambleRange)
            // );
            //
            // allyController.AddOrder(new MoveOrder(allyController.GetPosition() + destination,
            //     config.shambleSpeed));
            // allyController.AddOrder(new WaitOrder(Random.Range(config.shambleWait.x, config.shambleWait.y)));
            return new FollowOrder(allyController.GetPlayer(), config.followSpeed, config.haltDistance);
        }

        // Follow the player but stay outside of a certain range
        public static Order CreateFollowOrders(AllyController allyController, AllyManagerConfig config)
        {
            return new FollowOrder(allyController.GetPlayer(), config.followSpeed, config.haltDistance);
        }


        public static Order CreateCombatOrders(AllyController allyController, AllyManagerConfig config)
        {
            Zombie threat = allyController.GetClosestTarget();
            if (threat != null  )
            {
                float distance = Vector2.Distance(threat.GetPosition(), allyController.GetPosition());

                if (distance <= config.combatRange)
                {
                    Debug.Log(AllyController.TAG + "Created Fire Order");
                    return new FireOrder(threat);
                }
            }
            // Combat Orders ->
            //    Follow Player
            //    Fire at target

            // GameCharacter target = allyController.GetTarget();
            // if (target)
            // {
            //     allyController.AddOrder(new ChaseOrder(target, config.combatSpeed));
            // }
            // else
            // {
            //     Vector2 destination = new Vector2(
            //         Random.Range(-config.shambleRange, config.shambleRange),
            //         Random.Range(-config.shambleRange, config.shambleRange)
            //     );
            //
            //     allyController.AddOrder(new MoveOrder(allyController.GetPosition() + destination,
            //         config.combatSpeed));
            // }
            Debug.Log(AllyController.TAG + "Created Follow Order");
            return new FollowOrder(allyController.GetPlayer(), config.combatSpeed, config.haltDistance);
        }
    }
}