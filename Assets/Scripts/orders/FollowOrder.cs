using character;

namespace orders
{
    public class FollowOrder : Order
    {
        public Player player;
        public float spd;
        public float haltDistance;

        public FollowOrder(Player player, float spd, float haltDistance)
        {
            this.player = player;
            this.spd = spd;
            this.haltDistance = haltDistance;
        }
    }
}