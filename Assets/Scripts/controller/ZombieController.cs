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
    public class ZombieController : AIController
    {
        [SerializeField]
        private ZombieManagerConfig config;

        [SerializeField]
        private TargetManager<GameCharacter> targetManager = new TargetManager<GameCharacter>();

        [SerializeField]
        private ZombieState currentState = ZombieState.Shamble;

        private ActivatedZone activatedZone;
        private Zombie controlledZombie;

        private ZombieManager zombieManager;

        private void Start()
        {
            base.Start();

            zombieManager = GetComponentInParent<ZombieManager>();

            controlledZombie = GetComponent<Zombie>();
            controlledZombie.OnCharacterDestroyed += _ => zombieManager.RemoveZombie(this);

            activatedZone = GetComponentInChildren<ActivatedZone>();
            activatedZone.OnTriggerEntered += OnEnteredChaseZone;
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
                CreateZombieOrders();
            }
            else
            {
                if (currentOrder == null && orders.Count > 0) currentOrder = orders.Dequeue();

                if (currentOrder != null) HandleOrder(currentOrder);
            }
        }

        private void OnDestroy()
        {
            foreach (var gameCharacter in targetManager.GetTargets())
                gameCharacter.OnCharacterDestroyed -= RemoveTarget;
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                // Do damage to player
            }

            var innocentController = other.gameObject.GetComponent<InnocentController>();
            if (innocentController) innocentController.ConvertToZombie();
        }

        // Getters
        public ZombieState GetState()
        {
            return currentState;
        }

        public GameCharacter GetTarget()
        {
            return targetManager.GetTarget();
        }
        
        public GameCharacter GetClosestTarget()
        {
            return targetManager.GetClosestTarget(GetPosition());
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
                SetState(ZombieState.Chase);
            else
                SetState(zombieManager.GetGlobalState());
        }

        public void SetSearchRange(float range)
        {
            activatedZone.SetRange(range);
        }


        private void OnEnteredChaseZone(Collider2D other)
        {
            var gc = other.GetComponent<GameCharacter>();
            if (gc != null && (gc.CompareTag("Player") || gc.CompareTag("Innocent") || gc.CompareTag("Ally")))
            {
                targetManager.AddTarget(gc);
                CalculateState();
                gc.OnCharacterDestroyed += RemoveTarget;
            }
        }


        private void RemoveTarget(GameCharacter gc)
        {
            if (currentOrder is ChaseOrder co && (co.target == gc || co.target == null)) DumpOrders();

            targetManager.RemoveTarget(gc);
            gc.OnCharacterDestroyed -= RemoveTarget;
            CalculateState();
        }

        protected override void HandleOrder(Order order)
        {
            switch (order)
            {
                case MoveOrder mo:
                {
                    if (Move(mo)) currentOrder = null;

                    break;
                }
                case WaitOrder wo:
                {
                    if (Wait(ref wo)) currentOrder = null;

                    break;
                }
                case ChaseOrder co:
                {
                    if (Chase(co)) currentOrder = null;

                    break;
                }
            }
        }


        private void CreateZombieOrders()
        {
            if (zombieManager.GetGlobalState() == ZombieState.Combat && GetState() != ZombieState.Combat)
            {
                SetState(ZombieState.Combat);
                SetSearchRange(config.combatSearchRange);
            }

            if (NeedsOrder())
                switch (GetState())
                {
                    case ZombieState.Shamble:
                        orders.Enqueue(ZombieOrderFactory.CreateShambleOrder(this, config));
                        orders.Enqueue(ZombieOrderFactory.CreateWaitOrder(this, config));
                        break;
                    case ZombieState.Chase:
                        orders.Enqueue(ZombieOrderFactory.CreateChaseOrders(this, config));
                        break;
                    case ZombieState.Combat:
                        orders.Enqueue(ZombieOrderFactory.CreateCombatOrders(this, config));
                        break;
                }
        }
    }
}