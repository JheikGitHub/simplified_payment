using PaymentPicPay.API.ValueObjects;

namespace PaymentPicPay.API.Models
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

    }
}
