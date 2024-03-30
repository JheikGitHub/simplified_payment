using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.Tests.Domain._Builder
{
    public class CustomerBuilder
    {
        public CustomerBuilder()
        {

        }

        public Customer CreateSendBuild()
        {
            return new Customer(
                "Send Test",
                new("Send@mail.com"),
                 "@Send123",
                new(1000),
                "12345678910");
        }

        public Customer CreateReceiveBuild()
        {
            return new Customer(
                "Receive Test",
                new("Receive@mail.com"),
                 "@Receive123",
                new(1000),
                "12345678911");
        }
    }
}
