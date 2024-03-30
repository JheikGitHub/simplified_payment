using PaymentPicPay.API.Domain.ValueObjects;

namespace PaymentPicPay.API.Domain.Models
{
    public class User : EntityBase
    {
        protected User() { }
        public User(string fullName, Email email, string password, Wallet wallet)
        {
            FullName = fullName;
            Email = email;
            Password = password;
            Wallet = wallet;
        }

        public string FullName { get; private set; }
        public Email Email { get; private set; }
        public string Password { get; private set; }
        public Wallet Wallet { get; private set; }

    }
}
