using System;
using System.Collections.Generic;
using System.Linq;
using character;
using controller.ai;
using managers;
using orders;
using UnityEngine;

namespace controller
{
    public class InnocentController : AIController
    {
        public event Action<InnocentController> OnNeedsOrders;
        public event Action<InnocentController, InnocentConvertedTo> OnConverted;

        private Innocent controlledInnocent;
        private ActivatedZone activatedZone = null;

        private readonly List<Zombie> threats = new List<Zombie>();
        private InnocentMode currentMode = InnocentMode.Neutral;
        public InnocentMode GetMode() => currentMode;

        public Zombie GetThreat() => threats.First();

        private void Start()
        {
            base.Start();

            controlledInnocent = GetComponent<Innocent>();
            activatedZone = GetComponentInChildren<ActivatedZone>();
            activatedZone.OnTriggerEntered += OnEnteredRunZone;
            activatedZone.OnTriggerExited += OnExitedRunZone;
        }

        public void SetMode(InnocentMode newMode)
        {
            if (newMode != currentMode)
            {
                currentMode = newMode;
                controlledInnocent.SetMode(currentMode);
                DumpOrders();
            }
        }

        public void ConvertToZombie()
        {
            OnConverted?.Invoke(this, InnocentConvertedTo.Zombie);
        }

        public void ConvertToAlly()
        {
            OnConverted?.Invoke(this, InnocentConvertedTo.Ally);
        }

        private void OnEnteredRunZone(Collider2D other)
        {
            if (other.GetComponent<GameCharacter>() is Zombie z)
            {
                if (!threats.Contains(z))
                {
                    threats.Add(z);
                }

                SetMode(InnocentMode.Running);
            }
        }

        private void OnExitedRunZone(Collider2D other)
        {
            if (other.GetComponent<GameCharacter>() is Zombie z)
            {
                threats.Remove(z);
                if (currentOrder is RunOrder ro && ro.target == z)
                {
                    DumpOrders();
                }

                if (threats.Count == 0)
                {
                    SetMode(InnocentMode.Neutral);
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
                case RunOrder ro:
                {
                    if (RunAway(ro))
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
                default:
                    Debug.Log("Unhandled order on [InnocentController]");
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                ConvertToAlly();
                Destroy(gameObject);
            }
            else if (other.gameObject.GetComponent<Zombie>())
            {
                ConvertToZombie();
                Destroy(gameObject);
            }
        }
    }
}