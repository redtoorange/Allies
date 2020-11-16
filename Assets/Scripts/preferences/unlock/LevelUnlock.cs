using System;

namespace preferences
{
    [Serializable]
    public class LevelUnlock
    {
        public int levelCount;
        public bool[] unlockedLevels;

        public static LevelUnlock NewSave(int count)
        {
            LevelUnlock newSave = new LevelUnlock();

            newSave.unlockedLevels = new bool[count];
            newSave.levelCount = count;

            for (int i = 0; i < count; i++)
            {
                newSave.unlockedLevels[i] = false;
            }

            return newSave;
        }
    }
}