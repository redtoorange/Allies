using managers;
using UnityEngine;

namespace ui
{
    public class UIManager : MonoBehaviour
    {
        private UIController uiController;


        private SystemManager systemManager;

        private void Awake()
        {
            uiController = GetComponentInChildren<UIController>();

            systemManager = FindObjectOfType<SystemManager>();
        }

        public UIController GetUIController()
        {
            return uiController;
        }

        public SystemManager GetSystemManager()
        {
            return systemManager;
        }
    }
}