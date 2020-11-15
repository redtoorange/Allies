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
        public static readonly string TAG = "[AllyController]";

        // Events
        public event Action<AllyController> OnDeath;
        public event Action<AllyController> OnConverted;

        [SerializeField]
        private AllyState currentState = AllyState.Follow;
        [SerializeField]
        private AllyConfig config = null;

        private Ally controlledAlly = null;
        private ActivatedZone firingZone = null;
        private Player followedPlayer = null;
        private AllyManager manager = null;


        private TargetManager<Zombie> targetManager = new TargetManager<Zombie>();
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
            if (GameController.S.IsGamePaused()) return;
            
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
            if (GameController.S.IsGamePaused()) return;
            
            if (currentOrder != null)
            {
                HandleOrder(currentOrder);
            }
        }

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

        public AllyState GetState() => currentState;

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


        private void CreateAllyOrders()
        {
            switch (GetState())
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

        /// If a zombie exited the fire zone, it either died or moved out.  Check to see if it was the current target
        /// and Dump orders if needed.
        /// TODO Replace this with an Event?
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

        /// Convert the Ally into a Zombie. This triggers an Event dispatch and then Destroys the GO
        public void ConvertToZombie()
        {
            gameObject.SetActive(false);
            OnConverted?.Invoke(this);
            controlledAlly.DestroyingCharacter();
            Destroy(gameObject);
        }

        public Player GetPlayer() => followedPlayer;

        public Zombie GetClosestTarget() => targetManager.GetClosestTarget(GetPosition());
    }
}