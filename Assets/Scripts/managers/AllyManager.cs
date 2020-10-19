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

            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].OnConverted += OnAllyConverted;
            }
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                currentPhase = GameRoundPhase.Combat;
                currentGlobalState = AllyState.Combat;

                for (int i = 0; i < controllers.Count; i++)
                {
                    controllers[i].SetState(AllyState.Combat);
                }
            }
        }

        public void SpawnAlly(Vector2 position)
        {
            GameObject go = Instantiate(allyPrefab, position, Quaternion.identity, transform);
            AllyController allyController = go.GetComponent<AllyController>();

            AddController(go.GetComponent<AllyController>());
            gameRoundManager.SetCountDirty();
            
            allyController.OnConverted += OnAllyConverted;
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

        private void OnAllyConverted(AllyController allyController)
        {
            RemoveController(allyController);
            gameRoundManager.SetCountDirty();
            
            gameManager.GetZombieManager().SpawnZombie(allyController.GetPosition());
        }
    }
}