using PaymentPicPay.API.Enums;

namespace PaymentPicPay.API.ViewModels
{
    public class TransactionViewModel
    {
        public int SendId { get; set; }
        public int ReceiveId { get; set; }
        public decimal Amount { get; set; }
        public ETransactionType TransactionType { get; set; }
    }
}
