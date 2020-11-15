using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui.screens
{
    public class LoseScreen : MonoBehaviour
    {
        public void OnMainMenuClicked()
        {
            SceneManager.LoadScene(0);
        }

        public void OnRetryClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}