using System.Collections.Generic;
using preferences;
using preferences.unlock;
using UnityEngine;

namespace ui.levelSelect
{
    public class LevelListing : MonoBehaviour
    {
        [SerializeField]
        private LevelRow levelListingPrefab;

        [SerializeField]
        private List<LevelRowData> levelRowDataFiles;

        private List<LevelRow> listings;

        private void Start()
        {
            int levelCount = levelRowDataFiles.Count;

            LevelUnlock levelsUnlocked = LevelSaveSystem.LoadUnlocks();

            listings = new List<LevelRow>(levelCount);
            for (int i = 1; i <= levelCount; i++)
            {
                LevelRow row = Instantiate(levelListingPrefab, transform);

                row.SetData(levelRowDataFiles[i-1]);
                row.SetLevelUnlocked(levelsUnlocked.unlockedLevels[i]);

                listings.Add(row);
            }

            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100 * levelCount);
        }
    }
}