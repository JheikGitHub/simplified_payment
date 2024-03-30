using FluentValidation.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPicPay.API.Domain.Models
{
    public abstract class EntityBase : IEquatable<int>, IDisposable
    {
        public EntityBase()
        {
            CreatedDate = DateTime.UtcNow;
            Validations = new ValidationResult();
        }

        public int Id { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }

        [NotMapped]
        public ValidationResult Validations { get; protected set; }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public bool Equals(int other)
        {
            return Id == other;
        }

        public void UpdateDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
