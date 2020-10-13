using System;
using System.Collections.Generic;
using character;
using controller.ai;
using managers;
using orders;
using UnityEngine;
using util;

namespace controller
{
    public class ZombieController : AIController
    {
        public event Action<ZombieController> OnDeath;
        public event Action<ZombieController> OnNeedsOrders;

        [SerializeField]
        private TargetManager<GameCharacter> targetManager = new TargetManager<GameCharacter>();

        [SerializeField]
        private ZombieState currentState = ZombieState.Shamble;

        private ZombieManager zombieManager = null;
        private Zombie controlledZombie = null;
        private ActivatedZone activatedZone = null;

        // Getters
        public ZombieState GetState() => currentState;

        public GameCharacter GetTarget()
        {
            return targetManager.GetTarget();
        }

        private void Start()
        {
            base.Start();

            zombieManager = GetComponentInParent<ZombieManager>();
            controlledZombie = GetComponent<Zombie>();
            controlledZombie.OnCharacterDestroyed += (_) => OnDeath?.Invoke(this);

            activatedZone = GetComponentInChildren<ActivatedZone>();
            activatedZone.OnTriggerEntered += OnEnteredChaseZone;
            // activatedZone.OnTriggerExited += OnExitedChaseZone;
        }

        private void OnDisable()
        {
            activatedZone.OnTriggerEntered -= OnEnteredChaseZone;
            // activatedZone.OnTriggerExited -= OnExitedChaseZone;
        }
        
        private void OnDestroy()
        {
            foreach (GameCharacter gameCharacter in targetManager.GetTargets())
            {
                gameCharacter.OnCharacterDestroyed -= RemoveTarget;
            }
        }

        public void SetState(ZombieState state)
        {
            if (state != currentState)
            {
                currentState = state;
                controlledZombie.SetMode(currentState);
                DumpOrders();
            }
        }

        public void CalculateState()
        {
            if (targetManager.TargetCount() > 0)
            {
                SetState(ZombieState.Chase);
            }
            else
            {
                SetState(zombieManager.GetGlobalState());
            }
        }

        public void SetSearchRange(float range)
        {
            activatedZone.SetRange(range);
        }


        private void OnEnteredChaseZone(Collider2D other)
        {
            GameCharacter gc = other.GetComponent<GameCharacter>();
            if (gc != null && (gc.CompareTag("Player") || gc.CompareTag("Innocent") || gc.CompareTag("Ally")))
            {
                targetManager.AddTarget(gc);
                CalculateState();
                gc.OnCharacterDestroyed += RemoveTarget;
            }
        }

       

        private void RemoveTarget(GameCharacter gc)
        {
            if (currentOrder is ChaseOrder co && (co.target == gc || co.target == null))
            {
                DumpOrders();
            }
            targetManager.RemoveTarget(gc);
            gc.OnCharacterDestroyed -= RemoveTarget;
            CalculateState();
        }

        // private void OnExitedChaseZone(Collider2D other)
        // {
        //     GameCharacter gc = other.GetComponent<GameCharacter>();
        //     if (gc)
        //     {
        //         targetManager.RemoveTarget(gc);
        //         if (currentOrder is ChaseOrder co && co.target == gc)
        //         {
        //             DumpOrders();
        //         }
        //
        //         CalculateState();
        //     }
        // }

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
                case MoveOrder mo:
                {
                    if (Move(mo))
                    {
                        currentOrder = null;
                    }

                    break;
                }
                case WaitOrder wo:
                {
                    if (Wait(ref wo))
                    {
                        currentOrder = null;
                    }

                    break;
                }
                case ChaseOrder co:
                {
                    if (Chase(co))
                    {
                        currentOrder = null;
                    }

                    break;
                }
            }
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                // Do damage to player
            }

            InnocentController innocentController = other.gameObject.GetComponent<InnocentController>();
            if (innocentController)
            {
                innocentController.ConvertToZombie();
            }
        }
    }
}