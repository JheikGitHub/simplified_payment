using PaymentPicPay.API.Domain.ValueObjects;

namespace PaymentPicPay.API.Domain.Models
{
    public class Merchant : User
    {
        protected Merchant() { }

        public Merchant(
            string fullName,
            Email email,
            string password,
            Wallet wallet,
            string cNPJ)
            : base(fullName, email, password, wallet)
        {
            CNPJ = cNPJ;
        }

        public string CNPJ { get; private set; }

        public ICollection<TransactionB2C> Transactions { get; set; } = [];

        public override bool IsValid()
        {
            return Validations.IsValid;
        }
    }
}
