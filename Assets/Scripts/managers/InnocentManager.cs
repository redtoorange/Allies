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

            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].OnConverted += OnInnocentConverted;
            }
        }

        protected override void HandlePhaseChange(GameRoundPhase phase)
        {
            if (phase == GameRoundPhase.Combat)
            {
                currentPhase = GameRoundPhase.Combat;
                globalInnocentState = InnocentState.Combat;

                for (int i = 0; i < controllers.Count; i++)
                {
                    controllers[i].SetState(InnocentState.Combat);
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


        public InnocentState GetGlobalState()
        {
            return globalInnocentState;
        }
    }
}