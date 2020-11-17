using bullet;
using character;
using controller.audioController;
using managers;
using ui.health;
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
        private Player controlledPlayer;

        private Rigidbody2D rigidbody2D;

        private HealthBar playerHealthBar = null;
        private PlayerSoundController playerSoundController;

        private void Start()
        {
            camera = Camera.main;

            controlledPlayer = GetComponent<Player>();
            bulletManager = FindObjectOfType<BulletManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            playerSoundController = GetComponentInParent<PlayerSoundController>();

            playerHealthBar = GetComponentInParent<SystemManager>()
                .GetUIManager()
                .GetPlayerHealthBar();
            playerHealthBar.SetHealth(controlledPlayer.GetHealth());
        }

        private void Update()
        {
            if (GameController.S.IsGamePaused()) return;

            if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
            {
                Vector2 lookDirection = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                bulletManager.FireBullet(gameObject, rigidbody2D.position, lookDirection.normalized);
                playerSoundController.PlayShootSound();
                playerManager.CheckToStartCombat();
            }
        }

        private void FixedUpdate()
        {
            if (GameController.S.IsGamePaused()) return;

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

        public void TakeDamage(int amount)
        {
            playerSoundController.PlayHitSound();
            controlledPlayer.TakeDamage(amount);
            playerHealthBar.SetHealth(controlledPlayer.GetHealth());

            if (controlledPlayer.GetHealth() <= 0)
            {
                playerSoundController.PlayDeathSound();
                playerManager.PlayerDied();
            }
        }
    }
}