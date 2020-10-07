using System;
using System.Collections.Generic;
using System.Linq;
using character;
using controller.ai;
using orders;
using UnityEngine;

namespace controller
{
    public class ZombieController : AIController
    {
        public event Action<ZombieController> OnNeedsOrders; 
        
        private List<GameCharacter> targets = new List<GameCharacter>();

        [SerializeField]
        private ZombieMode currentMode = ZombieMode.SHAMBLE;

        private Zombie controlledZombie = null;
        private ActivatedZone activatedZone = null;

        // Getters
        public ZombieMode GetCurrentMode() => currentMode;
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

        public void SetCurrentMode(ZombieMode mode)
        {
            if (mode != currentMode)
            {
                currentMode = mode;
                controlledZombie.SetMode(currentMode);
                DumpOrders();
            }
        }


        private void OnEnteredChaseZone(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (currentMode != ZombieMode.COMBAT)
                {
                    SetCurrentMode(ZombieMode.CHASE);
                }

                if (!targets.Contains(other.GetComponent<Player>()))
                {
                    targets.Add(other.GetComponent<Player>());
                }
            }
            else if (other.CompareTag("Innocent"))
            {
                if (currentMode != ZombieMode.COMBAT)
                {
                    SetCurrentMode(ZombieMode.CHASE);
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

                if (currentMode != ZombieMode.COMBAT && targets.Count == 0)
                {
                    SetCurrentMode(ZombieMode.SHAMBLE);
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

                if (currentMode != ZombieMode.COMBAT && targets.Count == 0)
                {
                    SetCurrentMode(ZombieMode.SHAMBLE);
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