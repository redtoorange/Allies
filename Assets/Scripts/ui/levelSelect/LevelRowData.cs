using System;
using UnityEngine;

namespace ui.levelSelect
{
    [Serializable]
    [CreateAssetMenu(fileName = "Level_0", menuName = "levels/listing", order = 0)]
    public class LevelRowData : ScriptableObject
    {
        public Sprite levelImage;
        public string levelLabel;
        public int levelIndex;
    }
}