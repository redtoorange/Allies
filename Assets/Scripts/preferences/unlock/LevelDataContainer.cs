using ui.levelSelect;
using UnityEngine;

namespace preferences.unlock
{
    public class LevelDataContainer : MonoBehaviour
    {
        [SerializeField]
        private LevelData levelData;

        [SerializeField] [Tooltip("Reset all progress before saving")]
        private bool wipeSave = false;

        private void Start()
        {
            if (wipeSave)
            {
                LevelSaveSystem.ResetUnlocks();
            }

            LevelUnlock savedLevels = LevelSaveSystem.LoadUnlocks();
            bool modified = false;

            if (savedLevels != null)
            {
                if (GetLevelId() < savedLevels.levelCount)
                {
                    if (!savedLevels.unlockedLevels[GetLevelId()])
                    {
                        savedLevels.unlockedLevels[GetLevelId()] = true;
                        modified = true;
                        Debug.Log("Level Unlocked");
                    }
                    else
                    {
                        Debug.Log("Level already Unlocked");
                    }
                }
                else
                {
                    Debug.LogError("LevelId [" + GetLevelId() + "] out of bounds for LevelUnlock");
                }
            }
            else
            {
                Debug.LogError("LevelUnlock is null");
            }

            if (modified)
            {
                LevelSaveSystem.SaveUnlocks(savedLevels);
            }
        }

        public int GetLevelId()
        {
            return levelData.levelIndex;
        }
    }
}