using UnityEngine;

namespace scriptable
{
    [CreateAssetMenu(fileName = "InnocentManagerConfig", menuName = "Config/InnocentManager", order = 0)]
    public class InnocentConfig : ScriptableObject
    {
        [Header("Wander")]
        public float wanderSpeed = 1.0f;
        public float wanderRange = 2.0f;
        public Vector2 wanderWait = new Vector2(0.5f, 3.0f);

        [Header("Run")]
        public float runSpeed = 3.5f;

        [Header("Combat")]
        public float combatSpeed = 3.0f;
        public float combatRange = 1.0f;
    }
}