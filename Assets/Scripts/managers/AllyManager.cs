using System;
using character;
using controller;
using orders;
using scriptable;
using UnityEngine;

namespace managers
{
    [Serializable]
    public enum GlobalAllyState
    {
        Follow,
        Combat
    }

    public class AllyManager : AIManager<AllyController>
    {
        [SerializeField]
        private GameObject allyPrefab;

        [SerializeField]
        private AllyManagerConfig config;

        private readonly GlobalAllyState currentGlobalState = GlobalAllyState.Follow;

        private GameRoundPhase currentPhase = GameRoundPhase.Recruitment;


        private void Start()
        {
            base.Start();

            foreach (var allyController in controllers)
            {
                // Bind the callbacks
                allyController.OnDeath += RemoveController;
                allyController.OnNeedsOrders += CreateAllyOrders;
            }
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                currentPhase = GameRoundPhase.Combat;
                foreach (var ally in controllers)
                {
                    ally.SetState(AllyState.Combat);
                }
            }
        }

        private void CreateAllyOrders(AllyController allyController)
        {
            if (currentGlobalState == GlobalAllyState.Combat && allyController.GetCurrentState() != AllyState.Combat)
            {
                allyController.SetState(AllyState.Combat);
            }

            switch (allyController.GetCurrentState())
            {
                case AllyState.Neutral:
                    CreateNeutralOrders(allyController);
                    break;
                case AllyState.Follow:
                    CreateFollowOrders(allyController);
                    break;
                case AllyState.Combat:
                    CreateCombatOrders(allyController);
                    break;
            }
        }

        // What are neutral orders?  Should this be a small wander? Stay away from zombies?
        private void CreateNeutralOrders(AllyController allyController)
        {
            // Vector2 destination = new Vector2(
            //     Random.Range(-config.shambleRange, config.shambleRange),
            //     Random.Range(-config.shambleRange, config.shambleRange)
            // );
            //
            // allyController.AddOrder(new MoveOrder(allyController.GetPosition() + destination,
            //     config.shambleSpeed));
            // allyController.AddOrder(new WaitOrder(Random.Range(config.shambleWait.x, config.shambleWait.y)));
        }

        // Follow the player but stay outside of a certain range
        private void CreateFollowOrders(AllyController allyController)
        {
            var player = allyController.GetPlayer();
            if (player != null)
            {
                allyController.AddOrder(new FollowOrder(player, config.followSpeed, config.haltDistance));
            }
        }


        private void CreateCombatOrders(AllyController allyController)
        {
            // Combat Orders ->
            //    Follow Player
            //    Fire at target

            // GameCharacter target = allyController.GetTarget();
            // if (target)
            // {
            //     allyController.AddOrder(new ChaseOrder(target, config.combatSpeed));
            // }
            // else
            // {
            //     Vector2 destination = new Vector2(
            //         Random.Range(-config.shambleRange, config.shambleRange),
            //         Random.Range(-config.shambleRange, config.shambleRange)
            //     );
            //
            //     allyController.AddOrder(new MoveOrder(allyController.GetPosition() + destination,
            //         config.combatSpeed));
            // }
        }

        public void SpawnAlly(Vector2 position)
        {
            var go = Instantiate(allyPrefab, position, Quaternion.identity, transform);
            var allyController = go.GetComponent<AllyController>();

            AddController(go.GetComponent<AllyController>());

            // Bind the callbacks
            allyController.OnDeath += RemoveController;
            allyController.OnNeedsOrders += CreateAllyOrders;
        }
    }
}