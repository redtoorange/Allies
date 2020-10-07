namespace orders
{
    public class WaitOrder : Order
    {
        public float amount;

        public WaitOrder(float amount)
        {
            this.amount = amount;
        }
    }
}