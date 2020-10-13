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
    public class InnocentController : AIController
    {
        [SerializeField]
        private InnocentManagerConfig config;

        [SerializeField]
        private TargetManager<Zombie> targetManager = new TargetManager<Zombie>();

        private ActivatedZone activatedZone;
        private Innocent controlledInnocent;

        private InnocentState currentState = InnocentState.Neutral;

        private InnocentManager manager;

        private void Start()
        {
            base.Start();

            manager = GetComponentInParent<InnocentManager>();
            controlledInnocent = GetComponent<Innocent>();
            // controlledInnocent.OnCharacterDestroyed += HandleGameCharacterCharacterDestroyed;

            activatedZone = GetComponentInChildren<ActivatedZone>();
            activatedZone.OnTriggerEntered += OnEnteredRunZone;
            activatedZone.OnTriggerExited += OnExitedRunZone;
        }

        private void FixedUpdate()
        {
            if (NeedsOrder())
            {
                CreateInnocentOrders();
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

        public event Action<InnocentController, InnocentConvertedTo> OnConverted;

        public Zombie GetThreat() => targetManager.GetTarget();

        // Don't need to signal up to manager, innocent can only be converted, never die
        // private void HandleGameCharacterCharacterDestroyed(GameCharacter gc)
        // {
        // OnControllerDeath?.Invoke(this);
        // }

        public InnocentState GetState() => currentState;

        public void SetState(InnocentState newState)
        {
            if (newState == InnocentState.Neutral && manager.GetGlobalState() == InnocentState.Combat)
            {
                newState = InnocentState.Combat;
            }

            if (newState != currentState)
            {
                if (currentState == InnocentState.Running && targetManager.TargetCount() != 0)
                {
                    Debug.Log("Cannot changes state with threats in range");
                }
                else
                {
                    currentState = newState;
                    controlledInnocent.SetMode(currentState);
                    DumpOrders();
                }
            }
        }

        public void ConvertToZombie()
        {
            gameObject.SetActive(false);
            OnConverted?.Invoke(this, InnocentConvertedTo.Zombie);
            controlledInnocent.DestroyingCharacter();
            Destroy(gameObject);
        }

        public void ConvertToAlly()
        {
            gameObject.SetActive(false);
            OnConverted?.Invoke(this, InnocentConvertedTo.Ally);
            controlledInnocent.DestroyingCharacter();
            Destroy(gameObject);
        }

        private void OnEnteredRunZone(Collider2D other)
        {
            if (other.GetComponent<GameCharacter>() is Zombie z)
            {
                targetManager.AddTarget(z);
                SetState(InnocentState.Running);
            }
        }

        private void OnExitedRunZone(Collider2D other)
        {
            if (other.GetComponent<GameCharacter>() is Zombie z)
            {
                if (currentOrder is RunOrder ro && ro.target == z)
                {
                    DumpOrders();
                }

                targetManager.RemoveTarget(z);
                SetState(InnocentState.Neutral);
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
                case RunOrder ro:
                {
                    if (Run(ro))
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
                default:
                    Debug.Log("Unhandled order on [InnocentController]");
                    break;
            }
        }

        private void CreateInnocentOrders()
        {
            if (manager.GetGlobalState() == InnocentState.Combat && GetState() != InnocentState.Combat)
            {
                SetState(InnocentState.Combat);
            }

            switch (GetState())
            {
                case InnocentState.Neutral:
                    orders.Enqueue(InnocentOrderFactory.CreateWanderOrder(this, config));
                    orders.Enqueue(InnocentOrderFactory.CreateWaitOrder(this, config));
                    break;
                case InnocentState.Running:
                    orders.Enqueue(InnocentOrderFactory.CreateRunOrders(this, config));
                    break;
                case InnocentState.Combat:
                    orders.Enqueue(InnocentOrderFactory.CreateCombatOrders(this, config));
                    break;
            }
        }
    }
}