using System;
using UnityEngine;

namespace controller.ai
{
    public class ActivatedZone : MonoBehaviour
    {
        private CircleCollider2D chaseZone;

        public event Action<Collider2D> OnTriggerEntered;
        public event Action<Collider2D> OnTriggerExited;

        private void Start()
        {
            chaseZone = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEntered?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExited?.Invoke(other);
        }
    }
}