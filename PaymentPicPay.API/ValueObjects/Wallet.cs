namespace PaymentPicPay.API.ValueObjects
{
    public class Wallet
    {
        public Wallet(decimal balance)
        {
            Balance = balance;
        }

        public decimal Balance { get; private set; }

        public bool Payment(decimal amount)
        {
            if (HasBalance(amount))
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public void Receiving(decimal amount)
        {
            Balance += amount;
        }

        private bool HasBalance(decimal amount)
        {
            if (Balance >= amount)
                return true;
            return false;
        }
    }
}
