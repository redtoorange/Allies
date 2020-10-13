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

        public event Action<GameCharacter> OnCharacterDestroyed; 

        private Rigidbody2D rb2d = null;

        public Vector2 GetPosition()
        {
            if (rb2d != null)
            {
                return rb2d.position;
            }

            Debug.LogError("RigidBody2D has been destroyed on " + gameObject.name);
            return Vector2.zero;
        }

        protected void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(int amount)
        {
            stats.health -= amount;
            if (stats.health <= 0)
            {
                DestroyingCharacter();
                Destroy(gameObject);
            }
        }

        public void DestroyingCharacter()
        {
            OnCharacterDestroyed?.Invoke(this);
        }
    }
}