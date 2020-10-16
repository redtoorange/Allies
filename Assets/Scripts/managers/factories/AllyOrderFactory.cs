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
        public static Order CreateNeutralOrders(AllyController allyController, AllyConfig config)
        {
            return new FollowOrder(allyController.GetPlayer(), config.followSpeed, config.haltDistance);
        }

        // Follow the player but stay outside of a certain range
        public static Order CreateFollowOrders(AllyController allyController, AllyConfig config)
        {
            return new FollowOrder(allyController.GetPlayer(), config.followSpeed, config.haltDistance);
        }


        public static Order CreateFireOrder(AllyController allyController, AllyConfig config)
        {
            Zombie threat = allyController.GetClosestTarget();
            if (threat != null)
            {
                float distance = Vector2.Distance(threat.GetPosition(), allyController.GetPosition());

                if (distance <= config.combatRange)
                {
                    Debug.Log(AllyController.TAG + "Created Fire Order");
                    return new FireOrder(threat);
                }
            }

            return new WaitOrder(0);
        }
    }
}