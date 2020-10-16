using System.Collections.Generic;
using orders;
using UnityEngine;

namespace controller
{
    public abstract class AIController : MonoBehaviour
    {
        private readonly float movementThreshold = 0.1f;
        private readonly float stallThreshold = 0.01f;
        protected Order currentOrder;

        protected Queue<Order> orders = new Queue<Order>();
        private Vector2 positionLastFrame = new Vector2(float.MinValue, float.MinValue);
        protected Rigidbody2D rigidbody2D;


        protected void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnDisable()
        {
            DumpOrders();
        }

        private void OnDestroy()
        {
            DumpOrders();
        }

        public Vector2 GetPosition()
        {
            if (rigidbody2D == null)
            {
                return Vector2.negativeInfinity;
            }

            return rigidbody2D.position;
        }

        public bool NeedsOrder()
        {
            return orders.Count == 0 && currentOrder == null;
        }

        protected void DumpOrders()
        {
            orders.Clear();
            currentOrder = null;
        }

        public void AddOrder(Order order)
        {
            orders.Enqueue(order);
        }

        protected abstract void HandleOrder(Order order);

        protected bool Move(MoveOrder mo)
        {
            Vector2 position = rigidbody2D.position;
            Vector2 dir = mo.pos - position;
            rigidbody2D.MovePosition(position + dir.normalized * (mo.spd * Time.fixedDeltaTime));

            // Detect Stalled Movement
            if (Vector2.Distance(rigidbody2D.position, positionLastFrame) < stallThreshold)
            {
                positionLastFrame = new Vector2(float.MinValue, float.MinValue);
                return true;
            }

            positionLastFrame = rigidbody2D.position;

            return Vector2.Distance(positionLastFrame, mo.pos) < movementThreshold;
        }

        protected bool Chase(ChaseOrder co)
        {
            if (co.target == null || co.target.gameObject == null) return true;

            Vector2 position = rigidbody2D.position;
            Vector2 dir = co.target.GetPosition() - position;
            rigidbody2D.MovePosition(position + dir.normalized * (co.spd * Time.fixedDeltaTime));

            // Detect Stalled Movement
            if (Vector2.Distance(rigidbody2D.position, positionLastFrame) < stallThreshold)
            {
                Debug.Log("Stall Detected");
                positionLastFrame = new Vector2(float.MinValue, float.MinValue);
                return true;
            }

            positionLastFrame = rigidbody2D.position;

            return Vector2.Distance(positionLastFrame, co.target.GetPosition()) < movementThreshold;
        }

        protected bool Wait(ref WaitOrder wo)
        {
            wo.amount -= Time.fixedDeltaTime;

            return wo.amount <= 0;
        }

        protected bool Run(RunOrder ro)
        {
            if (ro.target == null || ro.target.gameObject == null) return true;

            Vector2 position = rigidbody2D.position;
            Vector2 dir = position - ro.target.GetPosition();
            rigidbody2D.MovePosition(position + dir.normalized * (ro.spd * Time.fixedDeltaTime));

            // Detect Stalled Movement
            if (Vector2.Distance(rigidbody2D.position, positionLastFrame) < stallThreshold)
            {
                positionLastFrame = new Vector2(float.MinValue, float.MinValue);
                return true;
            }

            positionLastFrame = rigidbody2D.position;

            return Vector2.Distance(positionLastFrame, ro.target.GetPosition()) < movementThreshold;
        }

        protected bool Follow(FollowOrder fo)
        {
            if (fo.player == null || fo.player.gameObject == null) return true;

            if (Mathf.Abs(Vector2.Distance(rigidbody2D.position, fo.player.GetPosition())) > fo.haltDistance)
            {
                Vector2 position = rigidbody2D.position;
                Vector2 dir = fo.player.GetPosition() - position;
                rigidbody2D.MovePosition(position + dir.normalized * (fo.spd * Time.fixedDeltaTime));

                // Detect Stalled Movement
                if (Vector2.Distance(rigidbody2D.position, positionLastFrame) < stallThreshold)
                {
                    positionLastFrame = new Vector2(float.MinValue, float.MinValue);
                    return true;
                }

                positionLastFrame = rigidbody2D.position;

                return false;
            }

            return true;
        }
    }
}