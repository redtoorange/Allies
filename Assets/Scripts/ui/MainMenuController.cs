using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui
{
    public class MainMenuController : MonoBehaviour
    {
    
        public void StartGameClicked()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitClicked()
        {
            Application.Quit();
        }
    }
}