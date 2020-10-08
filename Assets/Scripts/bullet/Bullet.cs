﻿using UnityEngine;

namespace bullet
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float travelSpeed = 1.0f;

        [SerializeField]
        private float timeToLive = 3.0f;

        private GameObject ignore = null;
        private Vector2 startPosition = new Vector2(-1000, -1000);
        private Vector2 direction = Vector2.zero;

        private Rigidbody2D rigidbody2D = null;
        private bool fired = false;

        private void OnEnable()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
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

        private void FixedUpdate()
        {
            if (!fired) return;

            rigidbody2D.MovePosition(
                rigidbody2D.position + (direction.normalized * (travelSpeed * Time.fixedDeltaTime)));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject != ignore)
            {
                Debug.Log("Collided with " + other.gameObject.name);
                ResetBullet();
            }
        }
    }
}