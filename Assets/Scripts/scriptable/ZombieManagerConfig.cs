using UnityEngine;

namespace scriptable
{
    [CreateAssetMenu(fileName = "ZombieManagerConfig", menuName = "Config/ZombieManager", order = 0)]
    public class ZombieManagerConfig : ScriptableObject
    {
        [Header("Shamble")]
        public float shambleSpeed = 1.0f;
        public float shambleRange = 2.0f;
        public Vector2 shambleWait = new Vector2(0.5f, 3.0f);
        
        [Header("Chase")]
        public float chaseSpeed = 2.5f;
        public Vector2 chaseWait = new Vector2(0.25f, 0.5f);
        
        [Header("Combat")]
        public float combatSpeed = 5.0f;
    }
}