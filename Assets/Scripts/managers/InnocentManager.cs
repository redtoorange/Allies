using character;
using controller;
using orders;
using scriptable;
using UnityEngine;

namespace managers
{
    public enum InnocentConvertedTo
    {
        Ally,
        Zombie
    }

    public class InnocentManager : AIManager<InnocentController>
    {
        [SerializeField]
        private InnocentManagerConfig config;

        private void Start()
        {
            base.Start();

            foreach (InnocentController innocent in controllers)
            {
                innocent.OnConverted += OnInnocentConverted;
                innocent.OnNeedsOrders += OnNeedsOrderHandler;
            }
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                foreach (InnocentController innocent in controllers)
                {
                    innocent.SetMode(InnocentMode.Running);
                }
            }
        }

        private void OnInnocentConverted(InnocentController innocent, InnocentConvertedTo to)
        {
            RemoveController(innocent);
            if (to == InnocentConvertedTo.Ally)
            {
                gameManager.AllyManager.SpawnAlly(innocent.GetPosition());
            }
            else if (to == InnocentConvertedTo.Zombie)
            {
                gameManager.ZombieManager.SpawnZombie(innocent.GetPosition());
            }
        }

        private void OnNeedsOrderHandler(InnocentController innocent)
        {
            switch (innocent.GetMode())
            {
                case InnocentMode.Neutral:
                    CreateWanderOrders(innocent);
                    break;
                case InnocentMode.Running:
                    CreateRunOrders(innocent);
                    break;
            }
        }


        private void CreateWanderOrders(InnocentController innocentController)
        {
            Vector2 destination = new Vector2(
                Random.Range(-config.wanderRange, config.wanderRange),
                Random.Range(-config.wanderRange, config.wanderRange)
            );

            innocentController.AddOrder(new MoveOrder(innocentController.GetPosition() + destination,
                config.wanderSpeed));
            innocentController.AddOrder(new WaitOrder(Random.Range(config.wanderWait.x, config.wanderWait.y)));
        }

        private void CreateRunOrders(InnocentController innocentController)
        {
            if (innocentController.GetThreat() != null)
            {
                innocentController.AddOrder(new RunOrder(innocentController.GetThreat(), config.runSpeed));
            }
        }
    }
}