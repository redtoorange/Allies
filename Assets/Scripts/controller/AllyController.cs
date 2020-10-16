﻿using System;
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
        public static readonly string TAG = "[AllyController]";
        
        public event Action<AllyController> OnConverted;

        [SerializeField]
        private AllyConfig config;

        [SerializeField]
        private AllyState currentState = AllyState.Follow;

        [SerializeField]
        private TargetManager<Zombie> targetManager = new TargetManager<Zombie>();

        private Ally controlledAlly;

        private ActivatedZone firingZone;
        private Player followedPlayer;

        private AllyManager manager;

        private bool canShoot = true;
        private float shotCooldown = 0.0f;

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

        private void Update()
        {
            if (shotCooldown > 0)
            {
                shotCooldown -= Time.deltaTime;
                if (shotCooldown <= 0)
                {
                    shotCooldown = 0;
                    canShoot = true;
                }
            }
            
            CalculateState();

            if (NeedsOrder())
            {
                CreateAllyOrders();
            }

            if (currentOrder == null && orders.Count > 0)
            {
                currentOrder = orders.Dequeue();
            }
        }

        private void FixedUpdate()
        {
            if (currentOrder != null)
            {
                HandleOrder(currentOrder);
            }
        }

        // Events
        public event Action<AllyController> OnDeath;

        private void CalculateState()
        {
            if (canShoot && GetClosestTarget() != null && manager.GetGlobalState() == AllyState.Combat)
            {
                SetState(AllyState.Combat);
            }
            else
            {
                SetState(AllyState.Follow);
            }
        }

        public void SetState(AllyState state)
        {
            if (state != currentState)
            {
                DumpOrders();
                currentState = state;
            }
        }

        protected override void HandleOrder(Order order)
        {
            bool completed = false;
            switch (order)
            {
                case WaitOrder wo:
                    completed = Wait(ref wo);
                    break;
                case FollowOrder fo:
                    completed = Follow(fo);
                    break;
                case FireOrder fire:
                    completed = HandleFireOrder(fire);
                    break;
                default:
                    Debug.Log(TAG + "Unhandled order " + order);
                    break;
            }

            if (completed)
            {
                currentOrder = null;
            }
        }

        private bool HandleFireOrder(FireOrder fires)
        {
            manager.FireBullet(this, fires.target.GetPosition());
            
            canShoot = false;
            shotCooldown = config.shotCooldown;
            
            return true;
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
            switch (GetCurrentState())
            {
                case AllyState.Neutral:
                    orders.Enqueue(AllyOrderFactory.CreateNeutralOrders(this, config));
                    break;
                case AllyState.Follow:
                    orders.Enqueue(AllyOrderFactory.CreateFollowOrders(this, config));
                    break;
                case AllyState.Combat:
                    orders.Enqueue(AllyOrderFactory.CreateFireOrder(this, config));
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

        public void ConvertToZombie()
        {
            gameObject.SetActive(false);
            OnConverted?.Invoke(this);
            controlledAlly.DestroyingCharacter();
            Destroy(gameObject);
        }
    }
}