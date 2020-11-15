using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ui
{
    public class GameStartTimer : MonoBehaviour
    {
        public UnityEvent onTimerOut;

        [SerializeField]
        private TextMeshProUGUI textLabel = null;
    
        [SerializeField]
        private int timeAmount = 3;
    
        private float remainingTime = 0f;
        private bool triggered = false;

        private void Start()
        {
            remainingTime = timeAmount;
        }

        private void Update()
        {
            if (!triggered)
            {
                remainingTime -= Time.deltaTime;
                textLabel.text = remainingTime.ToString("0.0");
                if (remainingTime <= 0)
                {
                    onTimerOut.Invoke();
                    triggered = true;
                    gameObject.SetActive(false);
                }
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}