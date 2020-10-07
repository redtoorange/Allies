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

        private float timeOut;
        private float remainingTime;
        private int currentDisplayNumber;

        private bool triggered = false;
        private bool started = false;

        public UnityEvent OnTimeOut;

        private Dictionary<int, string> stringCache;

        private void Awake()
        {
            BuildStringCache();
            Reset();
        }

        private void BuildStringCache()
        {
            stringCache = new Dictionary<int, string>(cacheCount + 1);
            for (int i = 0; i <= cacheCount; i++)
            {
                stringCache.Add(i, i + " s");
            }
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

        public void Reset()
        {
            started = false;
            triggered = false;
            remainingTime = timeOut;
            currentDisplayNumber = Mathf.FloorToInt(remainingTime);
            textLabel.SetText(stringCache[currentDisplayNumber]);
        }

        public void SetTime(float time)
        {
            timeOut = time;
        }
    }
}