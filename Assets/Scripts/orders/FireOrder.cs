using character;

namespace orders
{
    public class FireOrder : Order
    {
        public GameCharacter target;

        public FireOrder(GameCharacter target)
        {
            this.target = target;
        }
    }
}