using System;
using character;
using controller;

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
        private GameRoundPhase currentPhase = GameRoundPhase.Recruitment;
        private InnocentState globalInnocentState = InnocentState.Neutral;

        private void Start()
        {
            base.Start();

            foreach (var innocent in controllers) innocent.OnConverted += OnInnocentConverted;
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                currentPhase = GameRoundPhase.Combat;
                globalInnocentState = InnocentState.Combat;

                foreach (var innocent in controllers) innocent.SetState(InnocentState.Combat);
            }
        }

        private void OnInnocentConverted(InnocentController innocent, InnocentConvertedTo to)
        {
            RemoveController(innocent);
            if (to == InnocentConvertedTo.Ally)
                gameManager.GetAllyManager().SpawnAlly(innocent.GetPosition());
            else if (to == InnocentConvertedTo.Zombie)
                gameManager.GetZombieManager().SpawnZombie(innocent.GetPosition());
        }


        public InnocentState GetGlobalState()
        {
            return globalInnocentState;
        }
    }
}