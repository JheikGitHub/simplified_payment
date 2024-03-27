using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Domain.ValueObjects;

namespace PaymentPicPay.Tests.Domain._Builder
{
    public class MerchantBuilder
    {
        public MerchantBuilder()
        {

        }

        private readonly string FullName = "Customer Test";
        private readonly Email Email = new("Customer@mail.com");
        private readonly string Password = "@Customer123";
        private readonly Wallet Wallet = new(1000);
        private readonly string CNPJ = "12345678910";

        public Merchant Build()
        {
            return new Merchant
                (FullName,
                Email,
                Password,
                Wallet,
                CNPJ);
        }
    }
}
