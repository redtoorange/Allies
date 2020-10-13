using System;
using character;
using controller.ai;
using managers;
using managers.factories;
using orders;
using scriptable;
using UnityEngine;
using util;

namespace controller
{
    public class AllyController : AIController
    {
        public static string TAG = "[AllyController]";
        
        [SerializeField]
        private AllyManagerConfig config;
        [SerializeField]
        private AllyState currentState = AllyState.Follow;

        // Events
        public event Action<AllyController> OnDeath;

        private AllyManager manager = null;
        private Ally controlledAlly = null;
        private Player followedPlayer = null;
        
        [SerializeField]
        private TargetManager<Zombie> targetManager = new TargetManager<Zombie>();
        private ActivatedZone firingZone = null;

        private void Start()
        {
            base.Start();

            manager = GetComponentInParent<AllyManager>();
            controlledAlly = GetComponent<Ally>();

            followedPlayer = FindObjectOfType<Player>();
            firingZone = GetComponentInChildren<ActivatedZone>();
            firingZone.OnTriggerEntered += OnEnteredFireZone;
            firingZone.OnTriggerExited += OnExitedFireZone;
        }

        private void FixedUpdate()
        {
            if (NeedsOrder())
            {
                CreateAllyOrders();
            }
            else
            {
                if (currentOrder == null && orders.Count > 0)
                    currentOrder = orders.Dequeue();

                if (currentOrder != null)
                    HandleOrder(currentOrder);
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
                        Debug.Log(TAG + "Completed Follow Order");
                        currentOrder = null;
                    }
                    break;
                }
                case FireOrder fire:
                {
                    if (HandleFireOrder(fire))
                    {
                        Debug.Log(TAG + "Completed Fire Order");
                        currentOrder = null;
                    }
                    break;
                }
                default:
                    Debug.Log(TAG + "Unhandled order");
                    break;
            }
            
        }

        private bool HandleFireOrder(FireOrder fires)
        {
            manager.FireBullet(this, fires.target.GetPosition());
            return true;
        }

        public void SetState(AllyState state)
        {
            if (state != currentState)
            {
                DumpOrders();
                currentState = state;
            }
        }

        public AllyState GetCurrentState()
        {
            return currentState;
        }

        public Player GetPlayer()
        {
            return followedPlayer;
        }

        public Zombie GetClosestTarget()
        {
            return targetManager.GetClosestTarget(GetPosition());
        }

        private void CreateAllyOrders()
        {
            if (manager.GetGlobalState() == AllyState.Combat && GetCurrentState() != AllyState.Combat)
            {
                SetState(AllyState.Combat);
            }

            switch (GetCurrentState())
            {
                case AllyState.Neutral:
                    orders.Enqueue(AllyOrderFactory.CreateNeutralOrders(this, config));
                    Debug.Log(TAG + "Adding Neutral Order");
                    break;
                case AllyState.Follow:
                    orders.Enqueue(AllyOrderFactory.CreateFollowOrders(this, config));
                    Debug.Log(TAG + "Adding Follow Order");
                    break;
                case AllyState.Combat:
                    orders.Enqueue(AllyOrderFactory.CreateCombatOrders(this, config));
                    Debug.Log(TAG + "Adding Combat Order");
                    break;
            }
        }

        private void OnEnteredFireZone(Collider2D other)
        {
            if (other.GetComponent<GameCharacter>() is Zombie z)
            {
                targetManager.AddTarget(z);
            }
        }

        private void OnExitedFireZone(Collider2D other)
        {
            if (other.GetComponent<GameCharacter>() is Zombie z)
            {
                if (currentOrder is RunOrder ro && ro.target == z)
                {
                    DumpOrders();
                }

                targetManager.RemoveTarget(z);
            }
        }
    }
}