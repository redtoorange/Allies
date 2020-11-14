using UnityEngine;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        public static GameController S;

        private void Awake()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                Debug.LogError("Cannot have two GameControllers");
                Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        [SerializeField]
        private bool gamePaused = true;

        public bool IsGamePaused()
        {
            return gamePaused;
        }

        public void SetGamePaused(bool paused)
        {
            gamePaused = paused;
        }
    }
}