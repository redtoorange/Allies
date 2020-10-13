using UnityEngine;

namespace bullet
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private int damageAmount = 1;

        [SerializeField]
        private float travelSpeed = 1.0f;

        [SerializeField]
        private float timeToLive = 3.0f;

        private Vector2 direction = Vector2.zero;
        private bool fired;

        private GameObject ignore;

        private Rigidbody2D rigidbody2D;
        private Vector2 startPosition = new Vector2(-1000, -1000);

        private void FixedUpdate()
        {
            if (!fired) return;

            rigidbody2D.MovePosition(
                rigidbody2D.position + direction.normalized * (travelSpeed * Time.fixedDeltaTime));
        }

        private void OnEnable()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject != ignore)
            {
                var d = other.GetComponent<IDamageable>();
                if (d != null)
                {
                    d.TakeDamage(damageAmount);
                    ResetBullet();
                }
            }
        }

        public void PrimeBullet(GameObject ignore, Vector2 startPosition, Vector2 direction)
        {
            this.ignore = ignore;
            this.startPosition = startPosition;
            transform.position = startPosition;
            this.direction = direction;
        }

        private void ResetBullet()
        {
            fired = false;

            ignore = null;
            startPosition = new Vector2(-1000, -1000);
            transform.position = startPosition;
            direction = Vector2.zero;
            gameObject.SetActive(false);
        }

        public void Fire()
        {
            fired = true;
            Invoke(nameof(ResetBullet), timeToLive);
        }
    }
}