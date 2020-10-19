using character;
using controller;
using orders;
using scriptable;
using UnityEngine;

namespace managers.factories
{
    public abstract class AllyOrderFactory
    {
        /// What should an Ally do when idle? TODO figure this one out
        public static Order CreateNeutralOrders(AllyController allyController, AllyConfig config)
        {
            return new FollowOrder(allyController.GetPlayer(), config.followSpeed, config.haltDistance);
        }

        /// Create an order to Follow the Player but stay outside of a certain range.
        public static Order CreateFollowOrders(AllyController allyController, AllyConfig config)
        {
            return new FollowOrder(allyController.GetPlayer(), config.followSpeed, config.haltDistance);
        }


        /// Create a super simple fire order for the Ally to shoot at a Zombie.  If the Zombie has been Destroyed then
        /// just create a NOP wait.
        public static Order CreateFireOrder(AllyController allyController, AllyConfig config)
        {
            Zombie threat = allyController.GetClosestTarget();
            if (threat != null)
            {
                float distance = Vector2.Distance(threat.GetPosition(), allyController.GetPosition());

                if (distance <= config.combatRange)
                {
                    return new FireOrder(threat);
                }
            }

            return new WaitOrder(0);
        }
    }
}