using UnityEngine;

namespace controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10.0f;

        private Rigidbody2D rigidbody2D = null;

        private void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
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