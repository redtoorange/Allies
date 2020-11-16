using UnityEngine;

namespace preferences
{
    public class LevelUnlocker : MonoBehaviour
    {
        [SerializeField]
        private int levelId;

        private void Start()
        {
            LevelUnlock savedLevels = LevelSaveSystem.LoadUnlocks();
            bool modified = false;

            if (savedLevels != null)
            {
                if (levelId < savedLevels.levelCount)
                {
                    if (!savedLevels.unlockedLevels[levelId])
                    {
                        savedLevels.unlockedLevels[levelId] = true;
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
                    Debug.LogError("LevelId [" + levelId + "] out of bounds for LevelUnlock");
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
    }
}