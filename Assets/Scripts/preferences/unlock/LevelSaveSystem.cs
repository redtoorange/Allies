using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace preferences.unlock
{
    public class LevelSaveSystem
    {
        public static void ResetUnlocks()
        {
            SaveUnlocks(LevelUnlock.NewSave(10));
        }

        public static void SaveUnlocks(LevelUnlock unlocks)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/levels.big";

            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, unlocks);
            stream.Close();
        }

        public static LevelUnlock LoadUnlocks()
        {
            string path = Application.persistentDataPath + "/levels.big";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                LevelUnlock unlocks = formatter.Deserialize(stream) as LevelUnlock;
                stream.Close();
                return unlocks;
            }
            else
            {
                LevelUnlock unlocks = LevelUnlock.NewSave(10);
                SaveUnlocks(unlocks);
                return unlocks;
            }
        }
    }
}