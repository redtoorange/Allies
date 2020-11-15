using UnityEngine;

namespace ui.manager
{
    public class ModalUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject winScreen;

        [SerializeField]
        private GameObject loseScreen;

        private void Start()
        {
            winScreen.SetActive(false);
            loseScreen.SetActive(false);
        }

        public void DisplayWinScreen()
        {
            winScreen.SetActive(true);
        }

        public void DisplayLoseScreen()
        {
            loseScreen.SetActive(true);
                
        }
    }
}