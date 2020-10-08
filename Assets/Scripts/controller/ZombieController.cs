using System;
using System.Collections.Generic;
using System.Linq;
using character;
using controller.ai;
using orders;
using UnityEngine;
using UnityEngine.Serialization;

namespace controller
{
    public class ZombieController : AIController
    {
        public event Action<ZombieController> OnNeedsOrders; 
        
        private List<GameCharacter> targets = new List<GameCharacter>();

        [FormerlySerializedAs("currentMode")]
        [SerializeField]
        private ZombieState currentState = ZombieState.Shamble;

        private Zombie controlledZombie = null;
        private ActivatedZone activatedZone = null;

        // Getters
        public ZombieState GetCurrentMode() => currentState;
        public GameCharacter GetTarget() => targets.First();

        private void Start()
        {
            base.Start();

            controlledZombie = GetComponent<Zombie>();
            activatedZone = GetComponentInChildren<ActivatedZone>();

            activatedZone.OnTriggerEntered += OnEnteredChaseZone;
            activatedZone.OnTriggerExited += OnExitedChaseZone;
        }

        private void OnDisable()
        {
            activatedZone.OnTriggerEntered -= OnEnteredChaseZone;
            activatedZone.OnTriggerExited -= OnExitedChaseZone;
        }

        public void SetCurrentMode(ZombieState state)
        {
            if (state != currentState)
            {
                currentState = state;
                controlledZombie.SetMode(currentState);
                DumpOrders();
            }
        }


        private void OnEnteredChaseZone(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (currentState != ZombieState.Combat)
                {
                    SetCurrentMode(ZombieState.Chase);
                }

                if (!targets.Contains(other.GetComponent<Player>()))
                {
                    targets.Add(other.GetComponent<Player>());
                }
            }
            else if (other.CompareTag("Innocent"))
            {
                if (currentState != ZombieState.Combat)
                {
                    SetCurrentMode(ZombieState.Chase);
                }

                if (!targets.Contains(other.GetComponent<Innocent>()))
                {
                    targets.Add(other.GetComponent<Innocent>());
                }
            }
        }

        private void OnExitedChaseZone(Collider2D other)
        {
            if (other.GetComponent<GameCharacter>() is Player p)
            {
                targets.Remove(p);
                if (currentOrder is ChaseOrder co && co.target == p)
                {
                    DumpOrders();
                }

                if (currentState != ZombieState.Combat && targets.Count == 0)
                {
                    SetCurrentMode(ZombieState.Shamble);
                    AddOrder(new WaitOrder(0.25f));
                }
            }
            else if (other.GetComponent<GameCharacter>() is Innocent inn)
            {
                targets.Remove(inn);
                if (currentOrder is ChaseOrder co && co.target == inn)
                {
                    DumpOrders();
                }

                if (currentState != ZombieState.Combat && targets.Count == 0)
                {
                    SetCurrentMode(ZombieState.Shamble);
                    AddOrder(new WaitOrder(0.25f));
                }
            }
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
                case MoveOrder mo:
                {
                    if (MoveTowards(mo))
                    {
                        currentOrder = null;
                    }

                    break;
                }
                case WaitOrder wo:
                {
                    if (WaitAround(ref wo))
                    {
                        currentOrder = null;
                    }

                    break;
                }
                case ChaseOrder co:
                {
                    if (ChaseTowards(co))
                    {
                        currentOrder = null;
                    }

                    break;
                }
            }
        }
    }
}