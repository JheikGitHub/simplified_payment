namespace PaymentPicPay.API.Domain.Models
{
    public class EntityBase : IEquatable<int>
    {
        public EntityBase()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public int Id { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }

        public bool Equals(int other)
        {
            return Id == other;
        }

        public void UpdateDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
