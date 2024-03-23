using PaymentPicPay.API.ValueObjects;

namespace PaymentPicPay.API.Models
{
    public class Customer : User
    {
        protected Customer() { }

        public Customer(
            string fullName,
            Email email,
            string password,
            Wallet wallet,
            string cPF)
            : base(fullName, email, password, wallet)
        {
            CPF = cPF;
        }

        public string CPF { get; private set; }
    }
}
