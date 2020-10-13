using character;

namespace orders
{
    public class ChaseOrder : Order
    {
        public float spd;
        public GameCharacter target;

        public ChaseOrder(GameCharacter target, float spd)
        {
            this.target = target;
            this.spd = spd;
        }
    }
}