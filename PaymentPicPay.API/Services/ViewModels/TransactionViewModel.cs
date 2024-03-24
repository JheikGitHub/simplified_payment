using PaymentPicPay.API.Domain.Enums;

namespace PaymentPicPay.API.Services.ViewModels
{
    public class TransactionViewModel
    {
        public int SendId { get; set; }
        public int ReceiveId { get; set; }
        public decimal Amount { get; set; }
        public ETransactionType TransactionType { get; set; }
    }
}
