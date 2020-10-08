using System;
using bullet;
using UnityEngine;

namespace character
{
    [Serializable]
    public struct StatLine
    {
        public int health;
    }

    public class GameCharacter : MonoBehaviour, IDamageable
    {
        [SerializeField]
        protected StatLine stats;

        public event Action OnDeath; 

        private Rigidbody2D rb2d = null;
        public Vector2 GetPosition() => rb2d.position;

        protected void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(int amount)
        {
            stats.health -= amount;
            if (stats.health <= 0)
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}