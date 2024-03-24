using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Services.Externals.EmailSendService
{
    public interface IEmailSendService : IDisposable
    {
        Task SendNotificationTransaction(User user);
    }
}
