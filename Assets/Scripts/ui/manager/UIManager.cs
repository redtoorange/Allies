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
        private HealthBar playerHealthBar;

        private void Awake()
        {
            modalUIController = GetComponentInChildren<ModalUIController>();
            recruitmentTimer = GetComponentInChildren<CountDownTimer>();
            innocentCounter = GetComponentInChildren<InnocentCounter>();
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

        public HealthBar GetPlayerHealthBar()
        {
            return playerHealthBar;
        }
    }
}