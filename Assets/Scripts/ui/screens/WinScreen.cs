using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui.screens
{
    public class WinScreen : MonoBehaviour
    {
        public void OnMainMenuClicked()
        {
            SceneManager.LoadScene(0);
        }
        
        public void OnNextMissionClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}