using System;
using UnityEngine;

namespace character
{
    [Serializable]
    public struct StatLine
    {
        public int health;
    }

    public class GameCharacter : MonoBehaviour
    {
        [SerializeField]
        private StatLine stats;

        private Rigidbody2D rb2d = null;
        public Vector2 GetPosition() => rb2d.position;

        protected void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
    }
}