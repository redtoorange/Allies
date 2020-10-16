using bullet;
using managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace controller
{
    public class PlayerController : MonoBehaviour
    {
        public static readonly string TAG = "[PlayerController]";

        [SerializeField]
        private float speed = 10.0f;

        private BulletManager bulletManager;
        private Camera camera;
        private PlayerManager playerManager;

        private Rigidbody2D rigidbody2D;

        private void Start()
        {
            camera = Camera.main;

            bulletManager = FindObjectOfType<BulletManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
            {
                Vector2 lookDirection = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                bulletManager.FireBullet(gameObject, rigidbody2D.position, lookDirection.normalized);
                playerManager.CheckToStartCombat();
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
                                         inputDelta.normalized * (Time.fixedDeltaTime * speed));
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            InnocentController innocentController = other.gameObject.GetComponent<InnocentController>();
            if (innocentController != null)
            {
                innocentController.ConvertToAlly();
            }
        }
    }
}