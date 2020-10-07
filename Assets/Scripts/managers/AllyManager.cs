using controller;
using UnityEngine;

namespace managers
{
    public class AllyManager : AIManager<AllyController>
    {
        [SerializeField]
        private GameObject allyPrefab;


        private void Start()
        {
            base.Start();
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
        }

        public void SpawnAlly(Vector2 position)
        {
            GameObject go = Instantiate(allyPrefab, position, Quaternion.identity, transform);
            AddController(go.GetComponent<AllyController>());
        }
    }
}