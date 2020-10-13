using character;

namespace orders
{
    public class RunOrder : Order
    {
        public float spd;
        public GameCharacter target;

        public RunOrder(GameCharacter target, float spd)
        {
            this.target = target;
            this.spd = spd;
        }
    }
}