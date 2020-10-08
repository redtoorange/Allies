using bullet;
using UnityEngine;
using UnityEngine.UIElements;

namespace controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10.0f;

        private Rigidbody2D rigidbody2D = null;
        private BulletManager bulletManager = null;
        private Camera camera = null;

        private void Start()
        {
            camera = Camera.main;
            
            bulletManager = FindObjectOfType<BulletManager>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
            {
                Vector2 lookDirection = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                bulletManager.FireBullet(gameObject, rigidbody2D.position, lookDirection.normalized);
            }
        }

        private void FixedUpdate()
        {
            Vector2 inputDelta = Vector2.zero;

            inputDelta.x = Input.GetAxisRaw("Horizontal");
            inputDelta.y = Input.GetAxisRaw("Vertical");

            if (inputDelta.sqrMagnitude > 0)
            {
                rigidbody2D.MovePosition(rigidbody2D.position +
                                         (inputDelta.normalized * (Time.fixedDeltaTime * speed)));
            }
        }
    }
}