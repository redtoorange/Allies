using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui.screens
{
    public class GameOverScreen : MonoBehaviour
    {
        public void OnMainMenuClicked()
        {
            SceneManager.LoadScene(0);
        }
    }
}
