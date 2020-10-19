using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ui
{
    public class InnocentCounter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textLabel = null;
        [SerializeField]
        private int cacheCount = 99;

        private Dictionary<int, string> stringCache = null;

        private void Awake()
        {
            BuildCache();
        }

        private void BuildCache()
        {
            stringCache = new Dictionary<int, string>(cacheCount + 1);
            for (int i = 0; i <= cacheCount; i++)
            {
                stringCache.Add(i, i.ToString());
            }
        }

        public void SetCounter(int count)
        {
            textLabel.text = stringCache[count];
        }
    }
}