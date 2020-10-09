using character;

namespace orders
{
    public class ChaseOrder : Order
    {
        public GameCharacter target;
        public float spd;

        public ChaseOrder(GameCharacter target, float spd)
        {
            this.target = target;
            this.spd = spd;
        }
    }
}