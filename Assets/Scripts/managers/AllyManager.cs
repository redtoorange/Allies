using bullet;
using character;
using controller;
using UnityEngine;

namespace managers
{
    public class AllyManager : AIManager<AllyController>
    {
        [SerializeField]
        private GameObject allyPrefab;

        private BulletManager bulletManager;

        private AllyState currentGlobalState = AllyState.Follow;
        private GameRoundPhase currentPhase = GameRoundPhase.Recruitment;


        private void Start()
        {
            base.Start();

            bulletManager = gameManager.GetBulletManager();

            foreach (var allyController in controllers)
            {
                // Bind the callbacks
                allyController.OnDeath += RemoveController;
            }
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                currentPhase = GameRoundPhase.Combat;
                currentGlobalState = AllyState.Combat;
                foreach (var ally in controllers)
                {
                    ally.SetState(AllyState.Combat);
                }
            }
        }

        public void SpawnAlly(Vector2 position)
        {
            var go = Instantiate(allyPrefab, position, Quaternion.identity, transform);
            var allyController = go.GetComponent<AllyController>();

            AddController(go.GetComponent<AllyController>());

            // Bind the callbacks
            allyController.OnDeath += RemoveController;
        }

        public AllyState GetGlobalState()
        {
            return currentGlobalState;
        }

        public void FireBullet(AllyController controller, Vector2 targetPosition)
        {
            Vector2 dir = targetPosition - controller.GetPosition();
            bulletManager.FireBullet(gameObject, controller.GetPosition(), dir.normalized);
        }
    }
}