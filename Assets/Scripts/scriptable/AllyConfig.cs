using UnityEngine;

namespace scriptable
{
    [CreateAssetMenu(fileName = "AllyManagerConfig", menuName = "Config/AllyManager", order = 0)]
    public class AllyConfig : ScriptableObject
    {
        [Header("Follow")]
        public float followSpeed = 5f;
        public float haltDistance = 3.5f;

        [Header("Combat")]
        public float combatRange = 5.0f;
        public float shotCooldown = 1.0f;
    }
}