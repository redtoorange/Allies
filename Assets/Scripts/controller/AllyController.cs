using System;
using System.Collections.Generic;
using character;
using controller.ai;
using orders;
using UnityEngine;

namespace controller
{
    public class AllyController : AIController
    {
        public event Action<AllyController> OnDeath;
        public event Action<AllyController> OnNeedsOrders;

        private Player followedPlayer;
        private Ally controlledAlly;
        private List<Zombie> targets = new List<Zombie>();
        private ActivatedZone firingZone = null;
        private AllyState currentState = AllyState.Follow;

        private void Start()
        {
            base.Start();

            followedPlayer = FindObjectOfType<Player>();
        }

        private void FixedUpdate()
        {
            if (NeedsOrder())
            {
                OnNeedsOrders?.Invoke(this);
            }
            else
            {
                if (currentOrder == null && orders.Count > 0)
                {
                    currentOrder = orders.Dequeue();
                }

                if (currentOrder != null)
                {
                    HandleOrder(currentOrder);
                }
            }
        }

        protected override void HandleOrder(Order order)
        {
            switch (order)
            {
                case FollowOrder fo:
                {
                    if (Follow(fo))
                    {
                        currentOrder = null;
                    }

                    break;
                }
                default:
                    Debug.Log("Unhandled order on [AllyController]");
                    break;
            }
        }

        public void SetState(AllyState combat)
        {
        }

        public AllyState GetCurrentState()
        {
            return AllyState.Follow;
        }

        public Player GetPlayer()
        {
            return followedPlayer;
        }
    }
}