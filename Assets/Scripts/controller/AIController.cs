using System.Collections.Generic;
using orders;
using UnityEngine;

namespace controller
{
    public abstract class AIController : MonoBehaviour
    {
        private float movementThreshold = 0.1f;
        private float stallThreshold = 0.001f;
        private Vector2 positionLastFrame = new Vector2(float.MinValue, float.MinValue);

        protected Queue<Order> orders = new Queue<Order>();
        protected Order currentOrder = null;
        protected Rigidbody2D rigidbody2D = null;

        public Vector2 GetPosition() => rigidbody2D.position;
        public bool NeedsOrder() => orders.Count == 0 && currentOrder == null;


        protected void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
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

        protected bool MoveTowards(MoveOrder mo)
        {
            Vector2 position = rigidbody2D.position;
            Vector2 dir = mo.pos - position;
            rigidbody2D.MovePosition(position + (dir.normalized * (mo.spd * Time.fixedDeltaTime)));

            // Detect if the Zombie has stalled
            if (Vector2.Distance(rigidbody2D.position, positionLastFrame) < stallThreshold)
            {
                return true;
            }

            positionLastFrame = rigidbody2D.position;
            return Vector2.Distance(positionLastFrame, mo.pos) < movementThreshold;
        }

        protected bool ChaseTowards(ChaseOrder co)
        {
            Vector2 position = rigidbody2D.position;
            Vector2 dir = co.target.GetPosition() - position;
            rigidbody2D.MovePosition(position + (dir.normalized * (co.spd * Time.fixedDeltaTime)));

            return Vector2.Distance(rigidbody2D.position, co.target.GetPosition()) < movementThreshold;
        }

        protected bool WaitAround(ref WaitOrder wo)
        {
            wo.amount -= Time.fixedDeltaTime;

            return wo.amount <= 0;
        }

        protected bool RunAway(RunOrder ro)
        {
            Vector2 position = rigidbody2D.position;
            Vector2 dir = position - ro.target.GetPosition();
            rigidbody2D.MovePosition(position + (dir.normalized * (ro.spd * Time.fixedDeltaTime)));

            return Vector2.Distance(rigidbody2D.position, ro.target.GetPosition()) < movementThreshold;
        }
    }
}