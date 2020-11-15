using managers;
using UnityEngine;

namespace ui.manager
{
    public class UIManager : MonoBehaviour
    {
        private ModalUIController modalUIController;
        private SystemManager systemManager;

        private CountDownTimer recruitmentTimer;
        private InnocentCounter innocentCounter;
        private GameStartTimer gameStartTimer;

        private void Awake()
        {
            modalUIController = GetComponentInChildren<ModalUIController>();
            recruitmentTimer = GetComponentInChildren<CountDownTimer>();
            innocentCounter = GetComponentInChildren<InnocentCounter>();
            gameStartTimer = GetComponentInChildren<GameStartTimer>();
            // lifeContainer = GetComponentInChildren<GameStartTimer>();

            systemManager = FindObjectOfType<SystemManager>();
        }

        public ModalUIController GetModalUIController()
        {
            return modalUIController;
        }

        public SystemManager GetSystemManager()
        {
            return systemManager;
        }

        public CountDownTimer GetRecruitmentTimer()
        {
            return recruitmentTimer;
        }

        public InnocentCounter GetInnocentCounter()
        {
            return innocentCounter;
        }

        public GameStartTimer GetGameStartTimer()
        {
            return gameStartTimer;
        }
    }
}