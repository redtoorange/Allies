using System;
using character;
using controller;
using orders;
using scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace managers
{
    [Serializable]
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
        private InnocentState globalInnocentState = InnocentState.Neutral;

        private void Start()
        {
            base.Start();

            foreach (InnocentController innocent in controllers)
            {
                innocent.OnConverted += OnInnocentConverted;
                innocent.OnNeedsOrders += OnNeedsOrderHandler;
                innocent.OnControllerDeath += RemoveController;
            }
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                currentPhase = GameRoundPhase.Combat;
                globalInnocentState = InnocentState.Combat;

                foreach (InnocentController innocent in controllers)
                {
                    innocent.SetState(InnocentState.Combat);
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
            if (globalInnocentState == InnocentState.Combat && innocent.GetState() != InnocentState.Combat)
            {
                innocent.SetState(InnocentState.Combat);
            }

            switch (innocent.GetState())
            {
                case InnocentState.Neutral:
                    CreateWanderOrders(innocent);
                    break;
                case InnocentState.Running:
                    CreateRunOrders(innocent);
                    break;
                case InnocentState.Combat:
                    CreateCombatOrders(innocent);
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
        }

        private void CreateCombatOrders(InnocentController innocentController)
        {
            Vector2 destination = new Vector2(
                Random.Range(-config.combatRange, config.combatRange),
                Random.Range(-config.combatRange, config.combatRange)
            );

            innocentController.AddOrder(new MoveOrder(innocentController.GetPosition() + destination,
                config.combatSpeed));
        }

        public InnocentState GetGlobalState()
        {
            return globalInnocentState;
        }
    }
}