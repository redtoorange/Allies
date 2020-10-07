using UnityEngine;

namespace orders
{
    public class MoveOrder : Order
    {
        public Vector2 pos;
        public float spd;

        public MoveOrder(Vector2 pos, float spd)
        {
            this.pos = pos;
            this.spd = spd;
        }
    }
}