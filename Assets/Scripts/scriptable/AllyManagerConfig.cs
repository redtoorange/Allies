using UnityEngine;

namespace scriptable
{
    [CreateAssetMenu(fileName = "AllyManagerConfig", menuName = "Config/AllyManager", order = 0)]
    public class AllyManagerConfig : ScriptableObject
    {
        [Header("Neutral")]
        public float neutralSpeed = 1.0f;
        public float neutralRange = 2.0f;
        
        [Header("Follow")]
        public float followSpeed = 3.5f;
        public float haltDistance = 3.5f;
        
        [Header("Combat")]
        public float combatSpeed = 3.0f;
        public float combatRange = 4.0f;
    }
}