using managers;
using ui.health;
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
        private HealthBar playerHealthBar;

        private void Awake()
        {
            modalUIController = GetComponentInChildren<ModalUIController>();
            recruitmentTimer = GetComponentInChildren<CountDownTimer>();
            innocentCounter = GetComponentInChildren<InnocentCounter>();
            gameStartTimer = GetComponentInChildren<GameStartTimer>();
            playerHealthBar = GetComponentInChildren<HealthBar>();

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

        public HealthBar GetPlayerHealthBar()
        {
            return playerHealthBar;
        }
    }
}