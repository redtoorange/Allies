using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ui
{
    public class CountDownTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private int cacheCount = 240;

        public UnityEvent OnTimeOut;
        private int currentDisplayNumber;
        private float remainingTime;
        private bool started;
        private Dictionary<int, string> stringCache;
        private float timeOut;
        private bool triggered;

        private void Awake()
        {
            BuildStringCache();
            Reset();
        }

        public void Reset()
        {
            started = false;
            triggered = false;
            remainingTime = timeOut;
            currentDisplayNumber = Mathf.FloorToInt(remainingTime);
            textLabel.SetText(stringCache[currentDisplayNumber]);
        }

        private void Update()
        {
            if (!triggered && started)
            {
                remainingTime -= Time.deltaTime;

                if (remainingTime <= 0)
                {
                    triggered = true;
                    remainingTime = 0;
                    OnTimeOut.Invoke();
                }

                UpdateText();
            }
        }

        private void BuildStringCache()
        {
            stringCache = new Dictionary<int, string>(cacheCount + 1);
            for (int i = 0; i <= cacheCount; i++)
            {
                stringCache.Add(i, i + " s");
            }
        }

        private void UpdateText()
        {
            if (remainingTime < currentDisplayNumber)
            {
                currentDisplayNumber = Mathf.FloorToInt(remainingTime);
                textLabel.SetText(stringCache[currentDisplayNumber]);
            }
        }

        public void StartTimer()
        {
            Reset();
            started = true;
        }

        public void SetTime(float time)
        {
            timeOut = time;
        }
    }
}