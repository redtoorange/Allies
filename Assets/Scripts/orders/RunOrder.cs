using character;
using UnityEngine;

namespace orders
{
    public class RunOrder : Order
    {
        public GameCharacter target;
        public float spd;

        public RunOrder(GameCharacter target, float spd)
        {
            this.target = target;
            this.spd = spd;
        }
    }
}