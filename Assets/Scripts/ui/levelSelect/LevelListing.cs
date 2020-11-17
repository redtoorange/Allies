using System.Collections.Generic;
using UnityEngine;

namespace ui.levelSelect
{
    public class LevelListing : MonoBehaviour
    {
        [SerializeField]
        private LevelRow levelListingPrefab;

        private List<LevelRow> listings;

        [SerializeField]
        private int levelCount = 10;

        private void Start()
        {
            listings = new List<LevelRow>(levelCount);
            for (int i = 0; i < levelCount; i++)
            {
                listings.Add(Instantiate(levelListingPrefab, transform));
            }
            
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100 * levelCount);
        }
    }
}