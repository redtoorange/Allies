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

        private GameRoundPhase currentPhase = GameRoundPhase.Recruitment;

        private void Start()
        {
            base.Start();

            foreach (InnocentController innocent in controllers)
            {
                innocent.OnConverted += OnInnocentConverted;
                innocent.OnNeedsOrders += OnNeedsOrderHandler;
                innocent.OnDeath += RemoveController;
            }
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                currentPhase = GameRoundPhase.Combat;
                foreach (InnocentController innocent in controllers)
                {
                    innocent.SetMode(InnocentState.Running);
                }
            }
        }

        private void OnInnocentConverted(InnocentController innocent, InnocentConvertedTo to)
        {
            RemoveController(innocent);
            if (to == InnocentConvertedTo.Ally)
            {
                gameManager.GetAllyManager().SpawnAlly(innocent.GetPosition());
            }
            else if (to == InnocentConvertedTo.Zombie)
            {
                gameManager.GetZombieManager().SpawnZombie(innocent.GetPosition());
            }
        }

        private void OnNeedsOrderHandler(InnocentController innocent)
        {
            switch (innocent.GetMode())
            {
                case InnocentState.Neutral:
                    CreateWanderOrders(innocent);
                    break;
                case InnocentState.Running:
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
            GameCharacter threat = innocentController.GetThreat();
            if (threat != null)
            {
                innocentController.AddOrder(new RunOrder(threat, config.runSpeed));
            }
            else if (currentPhase == GameRoundPhase.Combat)
            {
                Vector2 destination = new Vector2(
                    Random.Range(-config.combatRange, config.combatRange),
                    Random.Range(-config.combatRange, config.combatRange)
                );

                innocentController.AddOrder(new MoveOrder(innocentController.GetPosition() + destination,
                    config.combatSpeed));
            }
        }
    }
}